Imports Emgu.CV
Imports Emgu.CV.CvEnum
Imports Emgu.CV.Structure
Imports Emgu.CV.Face
Imports System.Data.SQLite
Imports System.Drawing
Imports System.IO
Imports Emgu.CV.UI
Imports Emgu.CV.Util
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports AForge.Video
Imports AForge.Video.DirectShow

Public Class Form1
    Private faceCascade As CascadeClassifier
    Private dbConnection As SQLiteConnection
    Private recognizer As EigenFaceRecognizer
    Private isRecognizerTrained As Boolean = False
    Private Const ImageWidth As Integer = 250
    Private Const ImageHeight As Integer = 250
    Private ipCameraUrl As String = "http://192.168.1.2:8080/photo.jpg"
    Private cts As CancellationTokenSource
    Private videoSource As VideoCaptureDevice
    Private currentFrame As Mat
    Private unknownsFolderPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "unknowns")
    Private Const DistanceThreshold As Double = 5000 ' يمكنك تعديل هذا القيمة حسب الحاجة
    Private Const SensitivityAdjustment As Double = 2.5
    Dim trys As Integer = 3
    Private currentInputSource As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            faceCascade = New CascadeClassifier("haarcascade_frontalface_default.xml")
            recognizer = New EigenFaceRecognizer()
            InitializeDatabase()
            TrainRecognizer()


            ' Ensure the "unknowns" folder exists
            If Not Directory.Exists(unknownsFolderPath) Then
                Directory.CreateDirectory(unknownsFolderPath)
            End If
        Catch ex As Exception
            MessageBox.Show("Error initializing the form: " & ex.Message)
        End Try
    End Sub

    Private Sub InitializeDatabase()
        Try
            Dim dbPath As String = "Data Source=faceData.db;Version=3;"
            dbConnection = New SQLiteConnection(dbPath)
            dbConnection.Open()

            Dim createTableQuery As String = "
            CREATE TABLE IF NOT EXISTS FaceData (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                FaceImage BLOB NOT NULL
            );"

            Using command As New SQLiteCommand(createTableQuery, dbConnection)
                command.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error initializing the database: " & ex.Message)
        End Try
    End Sub

    Private Sub TrainRecognizer()
        Dim labels As New VectorOfInt()
        Dim images As New VectorOfMat()
        Dim labelCounter As Integer = 0

        Dim command As New SQLiteCommand("SELECT Id, Name, FaceImage FROM FaceData", dbConnection)
        Using reader As SQLiteDataReader = command.ExecuteReader()
            While reader.Read()
                Dim id As Integer = reader("Id")
                Dim faceImageBytes As Byte() = DirectCast(reader("FaceImage"), Byte())
                Using ms As New MemoryStream(faceImageBytes)
                    Dim faceImage As Bitmap = New Bitmap(ms)
                    Dim faceMat As Mat = BitmapToMat(faceImage)
                    CvInvoke.Resize(faceMat, faceMat, New Size(ImageWidth, ImageHeight)) ' Resize the image to the required dimensions
                    faceMat = faceMat.Clone() ' Ensure the matrix is continuous
                    CvInvoke.CvtColor(faceMat, faceMat, ColorConversion.Bgr2Gray)

                    labels.Push(New Integer() {id})
                    images.Push(faceMat)
                End Using
                labelCounter += 1
            End While
        End Using

        If images.Size > 1 Then ' Ensure there are at least 2 images to train the model
            recognizer.Train(images, labels)
            isRecognizerTrained = True

        Else
            MessageBox.Show("Not enough data to train the recognizer.")
            isRecognizerTrained = False
        End If
    End Sub



    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Select Case cboSource.SelectedIndex
            Case 0 ' Image
                currentInputSource = "Image"
                btnUpload.PerformClick()
            Case 1

                trys = 1


            Case 2 ' Webcam
                currentInputSource = "Webcam"
                trys = 1
                StartWebcam()
        End Select
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        StopCamera()
    End Sub

    Private Sub StartWebcam()
        Try
            Dim videoDevices As New FilterInfoCollection(FilterCategory.VideoInputDevice)
            If videoDevices.Count > 0 Then
                videoSource = New VideoCaptureDevice(videoDevices(0).MonikerString)
                AddHandler videoSource.NewFrame, AddressOf CaptureFrame
                videoSource.Start()
            Else
                MessageBox.Show("No webcam found.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error starting the webcam: " & ex.Message)
        End Try
    End Sub
    Private Async Sub StopCamera()
        Try
            ' Stop the video source if it is running
            If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
                RemoveHandler videoSource.NewFrame, AddressOf CaptureFrame
                videoSource.SignalToStop()

                ' Use a task to wait for the stop on a background thread
                Await Task.Run(Sub() videoSource.WaitForStop())

                videoSource = Nothing
            End If

            ' Cancel the camera task if it exists
            If cts IsNot Nothing Then
                cts.Cancel()
                cts.Dispose()
                cts = Nothing
            End If

            ' Use Invoke to update UI elements
            Invoke(Sub()
                       MessageBox.Show("Camera stopped successfully.")
                   End Sub)
        Catch ex As Exception
            Invoke(Sub()
                       MessageBox.Show("Error stopping the camera: " & ex.Message)
                   End Sub)
        End Try
    End Sub


    Private Sub CaptureFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Try
            Dim frame As Bitmap = CType(eventArgs.Frame.Clone(), Bitmap)
            Dim mat As Mat = BitmapToMat(frame)
            currentFrame = mat.Clone() ' Store the current frame
            ProcessFrame(mat)
        Catch ex As Exception
            MessageBox.Show("Error capturing frame: " & ex.Message)
        End Try
    End Sub

    Private Sub ProcessFrame(frame As Mat)
        Try
            If frame Is Nothing OrElse frame.IsEmpty Then
                MessageBox.Show("Frame is null or empty")
                Return
            End If

            Dim attempts As Integer = 0
            Dim faces As Rectangle() = {}
            Dim grayFrame As New Mat()

            ' Try face detection up to 3 times
            While attempts < trys
                CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray)
                CvInvoke.EqualizeHist(grayFrame, grayFrame)

                faces = faceCascade.DetectMultiScale(grayFrame, 1.1, trackBarSensitivity.Invoke(Function() trackBarSensitivity.Value), Size.Empty)

                If faces.Length = 1 Then
                    Exit While ' One face detected, proceed with processing
                ElseIf faces.Length > 1 AndAlso currentInputSource <> "Webcam" Then
                    AdjustTrackBarValueSafe(-SensitivityAdjustment)
                ElseIf faces.Length = 0 AndAlso currentInputSource <> "Webcam" Then
                    AdjustTrackBarValueSafe(SensitivityAdjustment)
                End If

                attempts += 1
            End While

            ' If more than one face is still detected, notify the user
            If faces.Length > 1 Then
                Dim result As DialogResult = MessageBox.Show("More than one face detected. Do you want to continue saving the image?", "Multiple Faces Detected", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    Return
                End If
            End If

            For Each face As Rectangle In faces
                ' Validate face rectangle
                If face.Width > 0 AndAlso face.Height > 0 AndAlso face.X >= 0 AndAlso face.Y >= 0 AndAlso face.X + face.Width <= frame.Width AndAlso face.Y + face.Height <= frame.Height Then
                    ' Isolate the CvInvoke.Rectangle call to check if it causes the issue
                    Try
                        CvInvoke.Rectangle(frame, face, New MCvScalar(255, 0, 0), 2)
                    Catch ex As AccessViolationException
                        MessageBox.Show("Access violation error during rectangle drawing: " & ex.Message)
                        Continue For
                    End Try

                    ' Get the face region
                    Using faceRegion As New Mat(grayFrame, face)
                        If Not faceRegion.IsEmpty Then
                            CvInvoke.Resize(faceRegion, faceRegion, New Size(ImageWidth, ImageHeight)) ' Resize the image to the required dimensions
                            Using faceRegionClone As Mat = faceRegion.Clone() ' Ensure the matrix is continuous
                                Dim name As String = "Unknown"
                                If isRecognizerTrained Then
                                    ' Match with stored faces
                                    Dim result As FaceRecognizer.PredictionResult = recognizer.Predict(faceRegionClone) ' Ensure the matrix is continuous

                                    ' Check distance threshold
                                    If result.Distance <= DistanceThreshold Then
                                        name = If(result.Label = -1, "Unknown", GetLabelName(result.Label))
                                    End If
                                End If

                                Dim text As String = name
                                Dim fontFace As FontFace = FontFace.HersheySimplex
                                Dim fontScale As Double = 0.9
                                Dim thickness As Integer = 2

                                ' Manually estimate text size (width and height)
                                Dim textWidth As Integer = text.Length * 10 ' Approximation: 10 pixels per character
                                Dim textHeight As Integer = 20 ' Approximation: 20 pixels for text height

                                ' Define the background rectangle
                                Dim textBackground As New Rectangle(face.X, face.Y - textHeight - 10, textWidth, textHeight + 5)

                                ' Draw background rectangle
                                Try
                                    CvInvoke.Rectangle(frame, textBackground, New MCvScalar(255, 0, 0), -1)
                                Catch ex As AccessViolationException
                                    MessageBox.Show("Access violation error during text background drawing: " & ex.Message)
                                    Continue For
                                End Try

                                ' Draw the text
                                Try
                                    CvInvoke.PutText(frame, name, New Point(face.X, face.Y - 10), fontFace, fontScale, New MCvScalar(255, 255, 255), thickness, LineType.AntiAlias)
                                Catch ex As AccessViolationException
                                    MessageBox.Show("Access violation error during text drawing: " & ex.Message)
                                    Continue For
                                End Try

                                If name = "Unknown" Then
                                    If RadioButton1.Checked = True Then
                                        Try
                                            SaveUnknownFace(faceRegionClone.ToImage(Of Bgr, Byte)().Bitmap)
                                            SaveFaceData(InputBox("Name", "Unknown FACE"), faceRegionClone.ToImage(Of Bgr, Byte)().Bitmap)
                                        Catch ex As AccessViolationException
                                            MessageBox.Show("Access violation error during face saving: " & ex.Message)
                                        End Try
                                    End If
                                End If
                            End Using
                        End If
                    End Using
                Else
                    MessageBox.Show($"Invalid face rectangle detected: {face}")
                End If
            Next

            ImageBox1.Image = frame
        Catch ex As AccessViolationException
            ' Log the exception or show a message
            MessageBox.Show("Access violation error: " & ex.Message)
        Catch ex As Exception
            ' Log other exceptions
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub AdjustTrackBarValueSafe(adjustment As Double)
        If trackBarSensitivity.InvokeRequired Then
            trackBarSensitivity.Invoke(New Action(Of Double)(AddressOf AdjustTrackBarValueSafe), adjustment)
        Else
            trackBarSensitivity.Value += adjustment
        End If
    End Sub

    Private Function GetLabelName(label As Integer) As String
        Dim command As New SQLiteCommand("SELECT Name FROM FaceData WHERE Id = @Id", dbConnection)
        command.Parameters.AddWithValue("@Id", label)
        Using reader As SQLiteDataReader = command.ExecuteReader()
            If reader.Read() Then
                Return reader("Name").ToString()
            End If
        End Using
        Return "Unknown"
    End Function

    Private Function StreamToMat(stream As Stream) As Mat
        Dim bitmap As Bitmap = New Bitmap(stream)
        Return BitmapToMat(bitmap)
    End Function

    Private Function BitmapToMat(bitmap As Bitmap) As Mat
        ' Convert Bitmap to Image(Of Bgr, Byte) and then to Mat
        Dim img As New Image(Of Bgr, Byte)(bitmap)
        Return img.Mat
    End Function

    Private Sub SaveFaceData(name As String, faceImage As Bitmap)
        ' Resize the image to a fixed size before saving
        Dim img As New Image(Of Bgr, Byte)(faceImage)
        CvInvoke.Resize(img, img, New Size(ImageWidth, ImageHeight))
        Dim resizedImage As Bitmap = img.Bitmap

        Using ms As New MemoryStream()
            resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)
            Dim faceImageBytes As Byte() = ms.ToArray()

            Dim command As New SQLiteCommand("INSERT INTO FaceData_Temp (Name, FaceImage) VALUES (@Name, @FaceImage)", dbConnection)
            command.Parameters.AddWithValue("@Name", name)
            command.Parameters.AddWithValue("@FaceImage", faceImageBytes)
            command.ExecuteNonQuery()
        End Using

        TrainRecognizer() ' Re-train the recognizer with the new data
    End Sub

    Private Sub SaveUnknownFace(faceImage As Bitmap)
        Try
            ' Ensure the "unknowns" folder exists
            If Not Directory.Exists(unknownsFolderPath) Then
                Directory.CreateDirectory(unknownsFolderPath)
            End If

            ' Generate a unique file name
            Dim fileName As String = Path.Combine(unknownsFolderPath, $"unknown_{DateTime.Now:yyyyMMddHHmmssfff}.bmp")

            ' Save the unknown face image
            faceImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp)

            ' Display the count of unknown faces
            Dim unknownFilesCount As Integer = Directory.GetFiles(unknownsFolderPath).Length
            MessageBox.Show($"Unknown face saved. There are now {unknownFilesCount} unknown faces.", "Unknown Face Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Error saving unknown face: {ex.Message}")
        End Try
    End Sub

    Private Sub btnSaveFaceFromCam_Click(sender As Object, e As EventArgs)
        If currentFrame IsNot Nothing Then
            Dim grayFrame As New Mat()
            CvInvoke.CvtColor(currentFrame, grayFrame, ColorConversion.Bgr2Gray)
            CvInvoke.EqualizeHist(grayFrame, grayFrame)

            Dim faces As Rectangle() = faceCascade.DetectMultiScale(grayFrame, 1.1, trackBarSensitivity.Invoke(Function() trackBarSensitivity.Value), Size.Empty)
            If faces.Length > 0 Then
                Dim face As Rectangle = faces(0)
                Dim faceRegion As New Mat(grayFrame, face)
                CvInvoke.Resize(faceRegion, faceRegion, New Size(ImageWidth, ImageHeight)) ' Resize the image to the required dimensions
                faceRegion = faceRegion.Clone() ' Ensure the matrix is continuous
                Dim inputName As String = InputBox("Enter the name for the new face:", "New Face")
                If Not String.IsNullOrEmpty(inputName) Then
                    SaveFaceData(inputName, faceRegion.ToImage(Of Bgr, Byte)().Bitmap)
                    MessageBox.Show("Face data saved successfully from camera!")
                End If
            Else
                MessageBox.Show("No face detected!")
            End If
        Else
            MessageBox.Show("No frame available!")
        End If
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim img As Image = Image.FromFile(ofd.FileName)
                Dim mat As Mat = BitmapToMat(img)
                ProcessFrame(mat)
            End If
        End Using
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim img As Image = Image.FromFile(ofd.FileName)
                Dim mat As Mat = BitmapToMat(img)
                If mat IsNot Nothing AndAlso Not mat.IsEmpty Then
                    Dim grayFrame As New Mat()
                    CvInvoke.CvtColor(mat, grayFrame, ColorConversion.Bgr2Gray)
                    CvInvoke.EqualizeHist(grayFrame, grayFrame)

                    Dim faces As Rectangle() = faceCascade.DetectMultiScale(grayFrame, 1.1, trackBarSensitivity.Invoke(Function() trackBarSensitivity.Value), Size.Empty)
                    If faces.Length > 0 Then
                        Dim face As Rectangle = faces(0)
                        Dim faceRegion As New Mat(grayFrame, face)
                        CvInvoke.Resize(faceRegion, faceRegion, New Size(ImageWidth, ImageHeight)) ' Resize the image to the required dimensions
                        faceRegion = faceRegion.Clone() ' Ensure the matrix is continuous
                        SaveFaceData(txtName.Text, faceRegion.ToImage(Of Bgr, Byte)().Bitmap)
                        MessageBox.Show("Face data saved successfully!")
                    Else
                        MessageBox.Show("No face detected!")
                    End If
                End If
            End If
        End Using
    End Sub

    Private Sub CopyDataFromTempToFaceData()
        Using command As New SQLiteCommand("INSERT INTO FaceData (Name, FaceImage) SELECT Name, FaceImage FROM FaceData_Temp", dbConnection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub ClearTempData()
        Using command As New SQLiteCommand("DELETE FROM FaceData_Temp", dbConnection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        InitializeDatabase()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CopyDataFromTempToFaceData()
        ClearTempData()
    End Sub
End Class
