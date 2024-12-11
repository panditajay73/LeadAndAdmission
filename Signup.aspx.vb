Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports ASPSnippets.GoogleAPI
Imports System.Web.Script.Serialization
Partial Class Signup
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Dim SessionID As String = ""
    Dim AyID As String = ""
    Public Class GoogleProfile
        Public Property Id() As String
        Public Property Name() As String
        Public Property Picture() As String
        Public Property Email() As String
        Public Property Mobile() As String
        Public Property Verified_Email() As String
    End Class
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        GoogleConnect.ClientId = "Enter Your ClientID"
        GoogleConnect.ClientSecret = "Enter Your Secret key"
        GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split("?"c)(0)

        If Not Me.IsPostBack Then
            Dim code As String = Request.QueryString("code")
            If Not String.IsNullOrEmpty(code) Then
                Dim connect As GoogleConnect = New GoogleConnect()
                Dim json As String = connect.Fetch("me", code)
                Dim profile As GoogleProfile = New JavaScriptSerializer().Deserialize(Of GoogleProfile)(json)
                'lblId.Text = profile.Id
                txtName.Text = profile.Name
                txtEmail.Text = profile.Email
                txtMobile.Text = profile.Mobile
                'lblVerified.Text = profile.Verified_Email
                'imgProfile.ImageUrl = profile.Picture
                'pnlProfile.Visible = True
                'btnLogin.Enabled = False
            End If
        End If
        If Not IsPostBack Then
            SessionIdAndAyid()
            GenerateCaptchaImage()
            LoadPrograms()

        End If
    End Sub
    Protected Sub Login(ByVal sender As Object, ByVal e As EventArgs)
        GoogleConnect.Authorize("profile", "email", "https://www.googleapis.com/auth/user.phonenumbers.read")

    End Sub

   Private Sub SessionIdAndAyid()
        ' SQL query to match AcademicYearName before dash and EvenOdd = 1
        Dim query As String = "SELECT SessionID, AyID FROM Academic_Year " &
                              "WHERE LEFT(AcademicYearName, CHARINDEX('-', AcademicYearName) - 1) = (SELECT YEAR(GETDATE())) " &
                              "AND EvenOdd = 1"

        ' Database connection and command
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    ' Save SessionID and AyID in session variables
                    Session("SessionID") = reader("SessionID").ToString()
                    Session("AyID") = reader("AyID").ToString()
                End If

                reader.Close()
            End Using
        End Using
    End Sub

    'Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click

    '    ' Ensure that all necessary fields are filled before proceeding
    '    If String.IsNullOrEmpty(txtName.Text.Trim()) OrElse String.IsNullOrEmpty(txtMobile.Text.Trim()) OrElse _
    '       String.IsNullOrEmpty(txtEmail.Text.Trim()) OrElse String.IsNullOrEmpty(txtPassword.Text.Trim()) OrElse _
    '       String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()) OrElse String.IsNullOrEmpty(txtCaptcha.Text.Trim()) Then
    '        lblResult.Text = "Please fill in all required fields."
    '        Return
    '    End If

    '    ' Validate email format
    '    Dim emailRegex As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
    '    If Not Regex.IsMatch(txtEmail.Text.Trim(), emailRegex) Then
    '        lblResult.Text = "Please enter a valid email address."
    '        Return
    '    End If

    '    ' Validate phone number
    '    If Not Regex.IsMatch(txtMobile.Text.Trim(), "^\d{10}$") Then
    '        lblResult.Text = "Please enter a valid 10-digit phone number."
    '        Return
    '    End If

    '    ' Validate Password and Confirm Password
    '    If txtPassword.Text.Trim() <> txtConfirmPassword.Text.Trim() Then
    '        lblResult.Text = "Password and confirm password do not match."
    '        Return
    '    End If

    '    ' Validate Captcha
    '    Dim sessionCaptcha As String = TryCast(Session("Captcha"), String)
    '    If sessionCaptcha Is Nothing OrElse Not txtCaptcha.Text.Trim().Equals(sessionCaptcha, StringComparison.Ordinal) Then
    '        lblResult.Text = "Captcha is incorrect."
    '        GenerateCaptchaImage()
    '        Return
    '    End If

    '    ' Get the selected course and its Course ID
    '    Dim selectedCourse As String = GetSelectedCourse()
    '    If String.IsNullOrEmpty(selectedCourse) Then
    '        lblResult.Text = "Please select a course."
    '        Return
    '    End If
    '    Dim courseId As String = GetSelectedCourseId(selectedCourse)

    '    If String.IsNullOrEmpty(courseId) Then
    '        lblResult.Text = "Course not found."
    '        Return
    '    End If

    '    ' Database operations for registration
    '    Using con As New SqlConnection(connStr)
    '        con.Open()

    '        ' Check if the mobile or email already exists
    '        Dim checkQuery As String = "SELECT COUNT(*) FROM StuRegistration WHERE MobileNo = @MobileNo OR Email = @Email"
    '        Using checkCmd As New SqlCommand(checkQuery, con)
    '            checkCmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim())
    '            checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim())

    '            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
    '            If count > 0 Then
    '                lblResult.Text = "Mobile or email already exists."
    '                Return
    '            End If
    '        End Using

    '        ' Generate the next registration number
    '        Dim cmd As New SqlCommand("SELECT ISNULL(MAX(CAST(RegistrationNo AS INT)), 1000) + 1 FROM StuRegistration", con)
    '        Dim newRegistrationNo As Integer = Convert.ToInt32(cmd.ExecuteScalar())

    '        ' Insert the registration details into the database
    '        Dim insertQuery As String = "INSERT INTO StuRegistration (Dated, FirstName, MobileNo, Email, userpassword, Courseid, RegistrationNo, Applycourse,sessionid,ayid) " &
    '                                    "VALUES (@Dated, @FirstName, @MobileNo, @Email, @Password, @Courseid, @RegistrationNo, @Applycourse,@sessionid, @ayid)"

    '        Using insertCmd As New SqlCommand(insertQuery, con)
    '            ' Prepare SQL parameters
    '            insertCmd.Parameters.AddWithValue("@Dated", DateTime.Now)
    '            insertCmd.Parameters.AddWithValue("@FirstName", txtName.Text.Trim())
    '            insertCmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim())
    '            insertCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim())
    '            insertCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim())
    '            insertCmd.Parameters.AddWithValue("@Courseid", courseId)
    '            insertCmd.Parameters.AddWithValue("@RegistrationNo", newRegistrationNo)
    '            insertCmd.Parameters.AddWithValue("@Applycourse", selectedCourse)
    '            insertCmd.Parameters.AddWithValue("@sessionid", Session("SessionID"))
    '            insertCmd.Parameters.AddWithValue("@ayid", Session("AyID"))

    '            Try
    '                insertCmd.ExecuteNonQuery()
    '                ' Success flow: generate OTP, send email, redirect to OTP page
    '                Dim otp As String = GenerateOTP()
    '                If Not SendOTPEmail(txtEmail.Text.Trim(), otp) Then
    '                    lblResult.Text = "Failed to send OTP. Please try again."
    '                    Return
    '                End If

    '                ' Store OTP and user details in session
    '                Session("OTP") = otp
    '                Session("Email") = txtEmail.Text.Trim()
    '                Session("UserID") = GetRegistrationIDByEmail().ToString()
    '                Session("Name") = txtName.Text.Trim()
    '                Session("UserName") = txtName.Text.Trim()
    '                Session("Mobile") = txtMobile.Text.Trim()
    '                Session("Password") = txtPassword.Text.Trim()
    '                Session("CourseId") = courseId
    '                Session("SelectedCourse") = selectedCourse

    '                Response.Redirect("OTPVerificationRegister.aspx")
    '            Catch ex As Exception
    '                lblResult.Text = "Error during registration: " & ex.Message
    '            End Try
    '        End Using
    '    End Using
    'End Sub
    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click
        ' Ensure that all necessary fields are filled before proceeding
        If String.IsNullOrEmpty(txtName.Text.Trim()) OrElse String.IsNullOrEmpty(txtMobile.Text.Trim()) OrElse _
           String.IsNullOrEmpty(txtEmail.Text.Trim()) OrElse String.IsNullOrEmpty(txtPassword.Text.Trim()) OrElse _
           String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()) OrElse String.IsNullOrEmpty(txtCaptcha.Text.Trim()) Then
            lblResult.Text = "Please fill in all required fields."
            Return
        End If

        ' Validate email format
        Dim emailRegex As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
        If Not Regex.IsMatch(txtEmail.Text.Trim(), emailRegex) Then
            lblResult.Text = "Please enter a valid email address."
            Return
        End If

        ' Validate phone number
        If Not Regex.IsMatch(txtMobile.Text.Trim(), "^\d{10}$") Then
            lblResult.Text = "Please enter a valid 10-digit phone number."
            Return
        End If

        ' Validate Password and Confirm Password
        If txtPassword.Text.Trim() <> txtConfirmPassword.Text.Trim() Then
            lblResult.Text = "Password and confirm password do not match."
            Return
        End If

        ' Validate Captcha
        Dim sessionCaptcha As String = TryCast(Session("Captcha"), String)
        If sessionCaptcha Is Nothing OrElse Not txtCaptcha.Text.Trim().Equals(sessionCaptcha, StringComparison.Ordinal) Then
            lblResult.Text = "Captcha is incorrect."
            GenerateCaptchaImage()
            Return
        End If

        ' Get the selected course and its Course ID
        Dim selectedCourse As String = GetSelectedCourse()
        If String.IsNullOrEmpty(selectedCourse) Then
            lblResult.Text = "Please select a course."
            Return
        End If

        Dim courseId As String = GetSelectedCourseId(selectedCourse)
        If String.IsNullOrEmpty(courseId) Then
            lblResult.Text = "Course not found."
            Return
        End If

        ' Check if the mobile or email already exists
        Using con As New SqlConnection(connStr)
            con.Open()
            Dim checkQuery As String = "SELECT COUNT(*) FROM StuRegistration WHERE MobileNo = @MobileNo OR Email = @Email"
            Using checkCmd As New SqlCommand(checkQuery, con)
                checkCmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim())
                checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim())

                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    lblResult.Text = "Mobile or email already exists."
                    Return
                End If
            End Using
        End Using

        ' Generate OTP
        Dim otp As String = GenerateOTP()
        If Not SendOTPEmail(txtEmail.Text.Trim(), otp) Then
            lblResult.Text = "Failed to send OTP. Please try again."
            Return
        End If

        ' Store all data in session variables
        Session("OTP") = otp
        Dim url As String = "OTPVerificationRegister.aspx?" &
    "Name=" & Server.UrlEncode(txtName.Text.Trim()) & "&" &
    "Mobile=" & Server.UrlEncode(txtMobile.Text.Trim()) & "&" &
    "Email=" & Server.UrlEncode(txtEmail.Text.Trim()) & "&" &
    "Password=" & Server.UrlEncode(txtPassword.Text.Trim()) & "&" &
    "CourseId=" & Server.UrlEncode(courseId) & "&" &
    "SelectedCourse=" & Server.UrlEncode(selectedCourse)

        Response.Redirect(url)


        ' Redirect to OTP verification page
        'Response.Redirect("OTPVerificationRegister.aspx")
    End Sub

    Public Function GetRegistrationIDByEmail(ByVal email As String) As Integer

        ' Initialize the connection and SQL command
        Dim registrationID As Integer = 0
        Dim query As String = "SELECT RegistrationID FROM stuRegistration WHERE Email = @Email"

        ' Create connection object
        Using con As New SqlConnection(connStr)
            ' Create the SQL command
            Using cmd As New SqlCommand(query, con)
                ' Add parameter for email
                cmd.Parameters.AddWithValue("@Email", email)

                ' Open connection
                con.Open()

                ' Execute the query and get the RegistrationID
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), registrationID) Then
                    ' Successfully parsed the RegistrationID
                Else
                    ' Handle the case where no RegistrationID was found
                    registrationID = 0 ' or throw an exception if preferred
                End If
            End Using
        End Using

        ' Return the RegistrationID
        Return registrationID
    End Function
    ' Fetch CourseId by using Courseprefix from the drop-down
    Protected Function GetSelectedCourseIdByPrefix(ByVal selectedCoursePrefix As String) As String
        Dim query As String = "SELECT Courseid FROM Exam_Course WHERE Courseprefix = @Courseprefix"
        Dim courseId As String = String.Empty

        Using connection As New SqlConnection(connStr)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Courseprefix", selectedCoursePrefix)
                connection.Open()
                Dim result As Object = command.ExecuteScalar()
                If result IsNot Nothing Then
                    courseId = result.ToString()
                End If
            End Using
        End Using

        Return courseId
    End Function

    Private Function GenerateOTP() As String
        Dim random As New Random()
        Dim otp As String = random.Next(100000, 999999).ToString()
        Return otp
    End Function

    Private Function SendOTPEmail(ByVal email As String, ByVal otp As String) As Boolean
        Try
            Dim smtpClient As New SmtpClient("smtp.gmail.com")
            smtpClient.Credentials = New System.Net.NetworkCredential("pandeyajaysdr@gmail.com", "uzcf bbzr dzaf cbxb")
            smtpClient.Port = 587
            smtpClient.EnableSsl = True

            Dim mailMessage As New MailMessage()
            mailMessage.From = New MailAddress("pandeyajaysdr@gmail.com")
            mailMessage.To.Add(email)
            mailMessage.Subject = "Your OTP Code for Verification"

            ' Construct the email body with concatenation
            mailMessage.Body = "Dear " & txtName.Text & "," & vbCrLf & vbCrLf &
                               "We have received a request to verify your email address. Please use the OTP code below to complete the verification process:" & vbCrLf & vbCrLf &
                               otp & vbCrLf & vbCrLf &
                               "If you did not request this, please ignore this email or contact our support team." & vbCrLf & vbCrLf &
                               "Thank you," & vbCrLf &
                               "Saral Erp Solutions" & vbCrLf &
                               "support@yourcompany.com"

            mailMessage.IsBodyHtml = False

            smtpClient.Send(mailMessage)
            Return True
        Catch ex As SmtpException
            LabelMessage.Text = "SMTP Error: " & ex.Message
            LabelMessage.Visible = True
            Return False
        Catch ex As InvalidOperationException
            LabelMessage.Text = "Invalid Operation: " & ex.Message
            LabelMessage.Visible = True
            Return False
        Catch ex As Exception
            LabelMessage.Text = "An error occurred: " & ex.Message
            LabelMessage.Visible = True
            Return False
        End Try
    End Function


    ' Function to get the Course ID based on the selected course
    Protected Function GetSelectedCourseId(ByVal selectedCourse As String) As String
        ' Query to fetch the course ID based on the selected course name
        Dim query As String = "SELECT Courseid FROM Exam_Course WHERE Course = @Course"

        ' Initialize the course ID variable
        Dim courseId As String = String.Empty

        ' Database operations
        Using connection As New SqlConnection(connStr)
            Using command As New SqlCommand(query, connection)
                ' Add the selected course as a parameter
                command.Parameters.AddWithValue("@Course", selectedCourse)

                connection.Open()

                ' Execute the query and retrieve the course ID
                Dim result As Object = command.ExecuteScalar()
                If result IsNot Nothing Then
                    courseId = result.ToString()
                End If
            End Using
        End Using

        ' Return the course ID
        Return courseId
    End Function

    ' Function to get the selected course name from the inputProgram TextBox
    Protected Function GetSelectedCourse() As String
        ' Access the inputProgram TextBox and get the text the user has selected or entered
        Dim selectedCourse As String = inputProgram.Text.Trim()

        ' Ensure that the user has selected or entered a value
        If String.IsNullOrEmpty(selectedCourse) Then
            ' Optionally handle if no course is selected
            lblResult.Text = "Please select a course."
            Return String.Empty
        End If

        ' Return the selected course name
        Return selectedCourse
    End Function

    Private Function IsValueInDatabase(ByVal fieldType As String, ByVal fieldValue As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM StuRegistration WHERE " & fieldType & " = @Value"
        Using con As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Value", fieldValue)
                con.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Protected Sub CheckEmailExists(ByVal sender As Object, ByVal e As EventArgs)
        Dim email As String = txtemail.Text.Trim()
        If IsValueInDatabase("Email", email) Then
            ' Trigger custom alert
            txtEmail.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Email already exists. Please use another.');", True)
        End If
    End Sub

    Protected Sub CheckMobileExists(ByVal sender As Object, ByVal e As EventArgs)
        Dim mobile As String = txtmobile.Text.Trim()
        If IsValueInDatabase("MobileNo", mobile) Then
            ' Trigger custom alert
            txtMobile.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Mobile number already exists. Please use another.');", True)
        End If
    End Sub
  Private Sub LoadPrograms()
        ' SQL query to fetch courses for the current academic year
        Dim query As String = "SELECT EC.Course " &
                              "FROM Exam_course EC " &
                              "INNER JOIN Exam_coursesession ECS ON EC.CourseID = ECS.CourseID " &
                              "WHERE ECS.AcademicYear = (SELECT YEAR(GETDATE()))"

        Using connection As New SqlConnection(connStr)
            Using command As New SqlCommand(query, connection)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim programOptions As String = ""

                    ' Loop through each row in the result set
                    While reader.Read()
                        programOptions &= "<option value='" & reader("Course") & "' style='color:Black;'>" & reader("Course") & "</option>"
                    End While

                    ' Inject the options into the Literal control
                    litProgramOptions.Text = programOptions
                End Using
            End Using
        End Using
    End Sub




    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs)
        GenerateCaptchaImage()
    End Sub

    Private Sub GenerateCaptchaImage()
        Dim captchaText As String = GenerateRandomText(6)
        Session("Captcha") = captchaText

        Using bmp As New Bitmap(130, 40)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim random As New Random()

                g.Clear(Color.White)
                Dim font As New Font("Arial", 18, FontStyle.Bold)
                Dim brush As New SolidBrush(Color.Black)

                ' Add noise
                For i As Integer = 0 To 49
                    g.DrawRectangle(New Pen(Brushes.Gray, 0), random.Next(0, bmp.Width), random.Next(0, bmp.Height), 1, 1)
                Next

                g.DrawString(captchaText, font, brush, 10, 5)

                ' Convert the image to a base64 string
                Using ms As New MemoryStream()
                    bmp.Save(ms, ImageFormat.Jpeg)
                    Dim byteArray As Byte() = ms.ToArray()
                    Dim base64String As String = Convert.ToBase64String(byteArray)
                    CaptchaImage.ImageUrl = "data:image/jpeg;base64," & base64String
                End Using
            End Using
        End Using
    End Sub

    Private Function GenerateRandomText(ByVal length As Integer) As String
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim random As New Random()
        Dim result As New System.Text.StringBuilder()
        For i As Integer = 1 To length
            Dim idx As Integer = random.Next(0, chars.Length)
            result.Append(chars.Substring(idx, 1))
        Next
        Return result.ToString()
    End Function

    Private Sub ClearFields()
        txtName.Text = ""
        txtMobile.Text = ""
        txtEmail.Text = ""
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
    End Sub
End Class
