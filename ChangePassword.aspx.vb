Imports System.Data.SqlClient
Imports System.IO

Partial Class ChangePassword
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("UserID") Is Nothing OrElse Request.QueryString("Email") Is Nothing OrElse Request.QueryString("UserName") Is Nothing Then
                ' If not logged in, redirect to the login page
                Response.Redirect("Login.aspx")
            End If
            Dim userId As String = Request.QueryString("UserID")
            userIcon.ImageUrl = GetUserPhoto(UserID)
            LoadCurrentPassword()


            Dim userName As String = Request.QueryString("UserName")
            Dim email As String = Request.QueryString("Email")

            ' Construct the query string
            Dim queryString As String = "?UserID=" & userId & "&UserName=" & userName & "&Email=" & email

            ' Update links with query parameters
            dashboardLink.HRef = "UserDashboard.aspx" & queryString
            fillApplicationButton.HRef = "ApplicationForm.aspx" & queryString  ' Keep as it is since no navigation link is specified
            statusLink.HRef = "ApplicationStatus.aspx" & queryString
            forumLink.HRef = "DiscussionForum.aspx" & queryString
            previewLink.HRef = "ApplicationFormPreview.aspx" & queryString
            profileLink.HRef = "UserProfilePage.aspx" & queryString
            profileLink2.HRef = "UserProfilePage.aspx" & queryString
            changePass.HRef = "ChangePassword.aspx" & queryString
        End If
    End Sub
    Protected Function GetUserPhoto(ByVal userID As String) As String
        Dim defaultPhoto As String = "~/images/user.png"
        Dim photoPath As String = defaultPhoto

        Try
            Using conn As New SqlConnection("Data Source=(local);Initial Catalog=College;Integrated Security=True")
                Dim query As String = "SELECT Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID AND EssentialDocID = 1"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@StudentID", userID)
                    conn.Open()

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Dim docFileName As String = result.ToString()
                        Dim filePath As String = "~/images/" & docFileName

                        If File.Exists(Server.MapPath(filePath)) Then
                            photoPath = filePath
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Log or handle the exception
            photoPath = defaultPhoto
        End Try

        Return photoPath
    End Function
    Private Sub LoadCurrentPassword()
        Dim userId As String = Request.QueryString("UserID")

        If Not String.IsNullOrEmpty(userId) Then

            Using connection As New SqlConnection(connStr)
                connection.Open()
                Dim command As New SqlCommand("SELECT userpassword FROM stuRegistration WHERE Registrationid = @UserID", connection)
                command.Parameters.AddWithValue("@UserID", userId)

                Dim currentPassword As String = Convert.ToString(command.ExecuteScalar())

                If Not String.IsNullOrEmpty(currentPassword) Then
                    txtCurrentPassword.Text = currentPassword
                Else
                    lblMessage.Text = "Unable to retrieve current password."
                End If
            End Using
        End If
    End Sub

    Protected Sub btnChangePassword_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim userId As String = Session("UserID").ToString()
        Dim currentPassword As String = txtCurrentPassword.Text
        Dim newPassword As String = txtNewPassword.Text
        Dim confirmPassword As String = txtConfirmPassword.Text

        ' Basic validation
        If newPassword <> confirmPassword Then
            lblMessage.Text = "New passwords do not match."
            lblMessage.ForeColor = System.Drawing.Color.Red ' Set the color to red
            Return
        End If

        If String.IsNullOrEmpty(currentPassword) OrElse String.IsNullOrEmpty(newPassword) Then
            lblMessage.Text = "Please fill in all fields."
            lblMessage.ForeColor = System.Drawing.Color.Red ' Set the color to red
            Return
        End If

        ' Check the current password and update it if valid
        If ChangeUserPassword(userId, currentPassword, newPassword) Then
            lblMessage.Text = "Password changed successfully."
            lblMessage.ForeColor = System.Drawing.Color.Green ' Set the color to red

            ' Add a client-side script to redirect after 2 seconds
            ClientScript.RegisterStartupScript(Me.GetType(), "RedirectScript", "setTimeout(function() { window.location.href='UserDashboard.aspx'; }, 2000);", True)
        Else
            lblMessage.Text = "Current password is incorrect."
            lblMessage.ForeColor = System.Drawing.Color.Red ' Set the color to red
        End If
    End Sub

    Private Function ChangeUserPassword(ByVal userId As String, ByVal currentPassword As String, ByVal newPassword As String) As Boolean
        Dim isPasswordChanged As Boolean = False
        Using connection As New SqlConnection(connStr)
            connection.Open()
            Dim command As New SqlCommand("SELECT userpassword FROM stuRegistration WHERE Registrationid = @UserID", connection)
            command.Parameters.AddWithValue("@UserID", userId)

            Dim existingPassword As String = Convert.ToString(command.ExecuteScalar())

            If existingPassword = currentPassword Then
                ' Update password
                Dim updateCommand As New SqlCommand("UPDATE stuRegistration SET userpassword = @NewPassword WHERE Registrationid = @UserID", connection)
                updateCommand.Parameters.AddWithValue("@NewPassword", newPassword)
                updateCommand.Parameters.AddWithValue("@UserID", userId)
                updateCommand.ExecuteNonQuery()
                isPasswordChanged = True
            End If
        End Using
        Return isPasswordChanged
    End Function
End Class
