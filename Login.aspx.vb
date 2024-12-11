Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports ASPSnippets.GoogleAPI
Imports System.Web.Script.Serialization
Partial Class Login
    Inherits System.Web.UI.Page
    Public Class GoogleProfile
        Public Property Id() As String
        Public Property Name() As String
        Public Property Picture() As String
        Public Property Email() As String
        Public Property Mobile() As String
        Public Property Verified_Email() As String
    End Class
    Dim connectionString As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
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
                'txtEmail.Text = profile.Email
                Using con As New SqlConnection(connectionString)
                    Dim query As String = "SELECT Registrationid, firstName, MobileNo, Applycourse FROM StuRegistration WHERE Email = @Email"
                    Using cmd As New SqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@Email", profile.Email)
                        con.Open()

                        ' Execute the query and get the result
                        Dim reader As SqlDataReader = cmd.ExecuteReader()

                        If reader.HasRows Then
                            reader.Read()
                            ' Save the UserID and Student (name) in session
                            ' Get the required values from the database or input fields
                            Dim registrationID As String = reader("RegistrationID").ToString()
                            Dim userName As String = reader("firstName").ToString()
                            Dim email As String = profile.Email
                            Dim mobileNo As String = reader("MobileNo").ToString()
                            Dim applyCourse As String = reader("ApplyCourse").ToString()

                            'Session("UserID") = registrationID
                            'Session("UserName") = userName
                            'Session("Email") = email
                            ' Construct the query string
                            Dim queryString As String = String.Format("UserDashboard.aspx?UserID={0}&UserName={1}&Email={2}",
                                                                       Server.UrlEncode(registrationID),
                                                                       Server.UrlEncode(userName),
                                                                       Server.UrlEncode(email))

                            ' Redirect to User Dashboard with query string
                            Response.Redirect(queryString)

                        Else
                            lblResult.Text = "Invalid email."
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Email ID does not registered with us.');", True)
                        End If
                    End Using
                End Using
            End If
        End If
        If Not IsPostBack Then
            GenerateCaptchaImage()
        End If
    End Sub
    Protected Sub Login(ByVal sender As Object, ByVal e As EventArgs)
        GoogleConnect.Authorize("profile", "email", "https://www.googleapis.com/auth/user.phonenumbers.read")

    End Sub
    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs)
        GenerateCaptchaImage()
    End Sub
    Protected Sub btnSignIn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSignIn.Click
        ' Clear previous result messages
        lblResult.Text = ""

        ' Basic input validation
        If String.IsNullOrEmpty(txtEmail.Text.Trim()) OrElse _
           String.IsNullOrEmpty(txtPassword.Text.Trim()) OrElse _
           String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()) Then
            lblResult.Text = "Please fill in all fields."
            Return
        End If

        ' Validate email format
        Dim emailPattern As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
        If Not System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), emailPattern) Then
            lblResult.Text = "Please enter a valid email address."
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Please enter a valid email address.');", True)
            Return
        End If

        ' Check if the password and confirm password match
        If txtPassword.Text.Trim() <> txtConfirmPassword.Text.Trim() Then
            lblResult.Text = "Password and confirm password do not match."
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Password and confirm password do not match.');", True)
            Return
        End If

        ' Validate Captcha
        Dim sessionCaptcha As String = TryCast(Session("Captcha"), String)
        If sessionCaptcha Is Nothing OrElse Not txtCaptcha.Text.Trim().Equals(sessionCaptcha, StringComparison.Ordinal) Then
            lblResult.Text = "Captcha is incorrect."
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Captcha is incorrect.');", True)
            GenerateCaptchaImage() ' Regenerate captcha after failure
            Return
        End If

        ' Verify email and password against the database
        Using con As New SqlConnection(connectionString)
            Dim query As String = "SELECT Registrationid, firstName, MobileNo, Applycourse FROM StuRegistration WHERE Email = @Email AND userpassword = @Password"
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim())
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim())

                con.Open()

                ' Execute the query and get the result
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    reader.Read()
                    ' Save the UserID and Student (name) in session
                    ' Get the required values from the database or input fields
                    Dim registrationID As String = reader("RegistrationID").ToString()
                    Dim userName As String = reader("firstName").ToString()
                    Dim email As String = txtEmail.Text.Trim()
                    Dim mobileNo As String = reader("MobileNo").ToString()
                    Dim applyCourse As String = reader("ApplyCourse").ToString()

                    'Session("UserID") = registrationID
                    'Session("UserName") = userName
                    'Session("Email") = email
                    ' Construct the query string
                    Dim queryString As String = String.Format("UserDashboard.aspx?UserID={0}&UserName={1}&Email={2}",
                                                               Server.UrlEncode(registrationID),
                                                               Server.UrlEncode(userName),
                                                               Server.UrlEncode(email))

                    ' Redirect to User Dashboard with query string
                    Response.Redirect(queryString)

                Else
                    lblResult.Text = "Invalid email or password."
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Invalid email or password.');", True)
                End If
            End Using
        End Using
    End Sub


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
            mailMessage.Subject = "Your OTP Code"
            mailMessage.Body = "Your OTP code is: " & otp

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
        txtEmail.Text = ""
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
    End Sub
End Class
