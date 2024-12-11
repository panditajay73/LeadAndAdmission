Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Data

Partial Class OTPVerificationRegister
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Ensure email is set in session before rendering the page

            Dim name As String = Request.QueryString("Name")
            Dim mobile As String = Request.QueryString("Mobile")
            Dim email As String = Request.QueryString("Email")
            Dim password As String = Request.QueryString("Password")
            Dim courseId As String = Request.QueryString("CourseId")
            Dim selectedCourse As String = Request.QueryString("SelectedCourse")
            If Request.QueryString("Email") Is Nothing Then
                Response.Redirect("Signup.aspx") ' Redirect if no session exists
            End If

            emailLabel.Text = email
        End If
    End Sub
    Private Function GetBatchID(ByVal courseID As Integer, ByVal connStr As String) As Integer
        Dim batchID As Integer = 0

        Using con As New SqlConnection(connStr)
            con.Open()

            ' Step 1: Retrieve the duration for the given CourseID
            Dim duration As Integer = 0
            Dim durationQuery As String = "SELECT Duration FROM Exam_CourseSession WHERE CourseID = @CourseID"
            Using durationCmd As New SqlCommand(durationQuery, con)
                durationCmd.Parameters.AddWithValue("@CourseID", courseID)
                Dim result As Object = durationCmd.ExecuteScalar()
                If result IsNot Nothing Then
                    duration = Convert.ToInt32(result)
                Else
                    Throw New Exception("Duration not found for the given CourseID.")
                End If
            End Using

            ' Step 2: Calculate the Batch string using the current year and duration
            Dim currentYear As Integer = DateTime.Now.Year
            Dim batchRange As String = currentYear.ToString() & "-" & (currentYear + duration).ToString().Substring(2, 2) ' Get the last 2 digits of the end year

            ' Step 3: Retrieve the BatchID that matches the calculated batch range
            Dim batchQuery As String = "SELECT BatchID FROM Batch WHERE Batch LIKE @Batch"
            Using batchCmd As New SqlCommand(batchQuery, con)
                batchCmd.Parameters.AddWithValue("@Batch", "%" & batchRange & "%") ' Use the LIKE operator to match the batch range
                Dim result As Object = batchCmd.ExecuteScalar()
                If result IsNot Nothing Then
                    batchID = Convert.ToInt32(result)
                Else
                    Throw New Exception("BatchID not found for the current year and duration.")
                End If
            End Using
        End Using

        Return batchID
    End Function

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        ' Combine the entered OTP digits
        Dim enteredOtp As String = txtOtp1.Text & txtOtp2.Text & txtOtp3.Text & txtOtp4.Text & txtOtp5.Text & txtOtp6.Text

        ' Check if the session OTP matches the entered OTP
        If Session("OTP") IsNot Nothing AndAlso enteredOtp = Session("OTP").ToString() Then
            ' OTP is correct, insert data into database
            Using con As New SqlConnection(connStr)
                con.Open()
                Dim courseID As Integer = Convert.ToInt32(Request.QueryString("CourseID"))
                Dim batchID As Integer = GetBatchID(courseID, connStr).ToString()

                Using cmd As New SqlCommand("InsertStuRegistration", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@Dated", DateTime.Now)
                    cmd.Parameters.AddWithValue("@FirstName", Request.QueryString("Name"))
                    cmd.Parameters.AddWithValue("@MobileNo", Request.QueryString("Mobile"))
                    cmd.Parameters.AddWithValue("@Email", Request.QueryString("Email"))
                    cmd.Parameters.AddWithValue("@Password", Request.QueryString("Password"))
                    cmd.Parameters.AddWithValue("@Courseid", Request.QueryString("CourseId"))
                    cmd.Parameters.AddWithValue("@Applycourse", Request.QueryString("SelectedCourse"))
                    cmd.Parameters.AddWithValue("@sessionid", Session("SessionID"))
                    cmd.Parameters.AddWithValue("@ayid", Session("AyID"))
                    cmd.Parameters.AddWithValue("@BatchID", batchID)
                    cmd.Parameters.AddWithValue("@sem", 1)

                    Try
                        cmd.ExecuteNonQuery()

                        ' Fetch data from the StuRegistration table to prepare query parameters
                        Dim selectQuery As String = "SELECT Registrationid, Student, MobileNo, Applycourse " &
                                                    "FROM StuRegistration WHERE Email = @Email AND MobileNo = @MobileNo"
                        Using selectCmd As New SqlCommand(selectQuery, con)
                            selectCmd.Parameters.AddWithValue("@Email", Request.QueryString("Email"))
                            selectCmd.Parameters.AddWithValue("@MobileNo", Request.QueryString("Mobile"))
                            Using reader As SqlDataReader = selectCmd.ExecuteReader()
                                If reader.Read() Then
                                    ' Prepare query string parameters
                                    Dim queryString As String = "UserID=" & reader("Registrationid").ToString() & _
                                                                "&UserName=" & reader("Student").ToString() & _
                                                                "&Email=" & Request.QueryString("Email")

                                    ' Redirect to the dashboard and pass values in the query string
                                    Response.Redirect("UserDashboard.aspx?" & queryString)
                                End If
                            End Using
                        End Using


                    Catch ex As Exception
                        lblMessage.Text = "Error during registration: " & ex.Message
                    End Try
                End Using
            End Using
        Else
            ' OTP is incorrect, show error and redirect
            lblMessage.Text = "Invalid OTP. Please try again."
        End If
    End Sub


    Protected Sub ResendOTPClick(ByVal sender As Object, ByVal e As EventArgs)
        Session("OTP") = GenerateOTP()
        Dim otp As String = GenerateOTP()
        If Not SendOTPEmail(Request.QueryString("Email"), otp) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Failed to send OTP. Please try again.');", True)
            Return
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('OTP sended successfully.');", True)
    End Sub
    Private Function GenerateOTP() As String
        Dim random As New Random()
        Dim otp As String = random.Next(100000, 999999).ToString()
        Return otp
    End Function

    Private Function SendOTPEmail(ByVal email As String, ByVal otp As String) As Boolean
            Dim smtpClient As New SmtpClient("smtp.gmail.com")
            smtpClient.Credentials = New System.Net.NetworkCredential("pandeyajaysdr@gmail.com", "uzcf bbzr dzaf cbxb")
            smtpClient.Port = 587
            smtpClient.EnableSsl = True

            Dim mailMessage As New MailMessage()
            mailMessage.From = New MailAddress("pandeyajaysdr@gmail.com")
        mailMessage.To.Add(email)
        mailMessage.Subject = "Your OTP Code for Verification"

        ' Construct the email body with concatenation
        mailMessage.Body = "Dear " & Request.QueryString("Name") & "," & vbCrLf & vbCrLf &
                           "We have received a request to verify your email address. Please use the OTP code below to complete the verification process:" & vbCrLf & vbCrLf &
                           otp & vbCrLf & vbCrLf &
                           "If you did not request this, please ignore this email or contact our support team." & vbCrLf & vbCrLf &
                           "Thank you," & vbCrLf &
                           "Saral Erp Solutions" & vbCrLf &
                           "support@yourcompany.com"

        mailMessage.IsBodyHtml = False ' Use plain text

            smtpClient.Send(mailMessage)
            Return True
    End Function
End Class
