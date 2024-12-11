Imports System.Data.SqlClient

Partial Class AuthorityPages_Login
    Inherits System.Web.UI.Page

    Dim connectionString As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadRoles() ' Load distinct roles from the Login table into the RadioButtonList
        End If
    End Sub

    ' Load distinct roles from the Login table into the RadioButtonList
    Private Sub LoadRoles()
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT DISTINCT Role FROM Login" ' Assuming the table is called 'Login'
            Dim cmd As New SqlCommand(query, conn)

            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.HasRows Then
                rblRoles.DataSource = reader
                rblRoles.DataTextField = "Role"
                rblRoles.DataValueField = "Role"
                rblRoles.DataBind()
            End If
        End Using
    End Sub

    ' Validate email and password on sign-in
    Protected Sub btnSignIn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSignIn.Click
        Dim email As String = txtEmail.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Email and Password cannot be empty.');", True)
            Return
        End If

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM Login WHERE Email = @Email AND Password = @Password"
            Dim cmd As New SqlCommand(query, conn)

            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@Password", password)

            conn.Open()
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            ' If email and password are correct, show the role popup
            If count > 0 Then
                ' Store the email in session and open the role selection popup
                Session("Email") = email
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowPopup", "$('#roleModal').modal('show');", True)
            Else
                ' If email or password does not match, show an alert
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Email or Password does not match. Please try again.');", True)
            End If
        End Using
    End Sub

    ' Validate the role when selected from the radio button list
    Protected Sub rblRoles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblRoles.SelectedIndexChanged
        ' Check if the session is null before accessing it
        If Session("Email") IsNot Nothing Then
            Dim selectedRole As String = rblRoles.SelectedValue
            Dim email As String = Session("Email").ToString()

            ' Check if the selected role matches the role associated with the email in the Login table
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT Role FROM Login WHERE Email = @Email"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Email", email)

                conn.Open()
                Dim role As String = Convert.ToString(cmd.ExecuteScalar())

                ' Check if the fetched role matches the selected role
                If role IsNot Nothing Then
                    If role.Equals("HoD", StringComparison.OrdinalIgnoreCase) Then
                        Response.Redirect("Dashboard.aspx")
                    ElseIf role.Equals("Admin", StringComparison.OrdinalIgnoreCase) Then
                        Response.Redirect("AdminDashboard.aspx")
                    Else
                        ' If the role does not match, show an alert to select the correct role
                        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('You do not have permission to access this area.');", True)
                    End If
                Else
                    ' Handle case when the role is not found
                    ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Role not found.');", True)
                End If
            End Using
        Else
            ' Handle case when Session("Email") is null
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Session expired. Please log in again.');", True)
        End If
    End Sub

End Class
