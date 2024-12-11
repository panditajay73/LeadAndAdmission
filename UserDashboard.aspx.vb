Imports System.Data.SqlClient
Imports System.IO

Partial Class UserDashboard
    Inherits System.Web.UI.Page

    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Check if the user is logged in
            If Request.QueryString("UserID") Is Nothing OrElse Request.QueryString("Email") Is Nothing OrElse Request.QueryString("UserName") Is Nothing Then
                ' If not logged in, redirect to the login page
                Response.Redirect("Login.aspx")
            End If
            'Session("UserID") = Request.QueryString("UserID")
            'Session("UserName") = Request.QueryString("UserName")
            'Session("Email") = Request.QueryString("Email")
            UserNameLaabel.Text = Request.QueryString("UserName")
        End If
        Dim remark As String = GetRemarkByUserID()

        ' Check if the remark is available and update the Literal control
        If Not String.IsNullOrEmpty(remark) Then
           litRemarks.Text = remark
            remarks.Attributes("Class") = "notification-remarks"
            remarks.Attributes("onclick") = String.Format("window.location.href='ApplicationStatus.aspx?UserID={0}&UserName={1}&Email={2}';", Request.QueryString("UserID"), Request.QueryString("UserName"), Request.QueryString("Email"))
        Else
            litRemarks.Text = "No Feedback Available"
        End If
        ' Load and display the user photo
        Dim userId As String = GetRegistrationIDByEmail().ToString()
        userIcon.ImageUrl = GetUserPhoto(userId)

        ' Set percentage text
        litPercentage.Text = CalculateStudentPointsPercentageApplication(Request.QueryString("UserID")).ToString("0") & "%"  ' Format as an integer percentage
        profilePercentage.Text = CalculateStudentPointsPercentageUserProfile(Request.QueryString("UserID")).ToString("0") & "%"
        PopulateProfileDetails()
        Dim userName As String = Request.QueryString("UserName")
        Dim email As String = Request.QueryString("Email")

        ' Construct the query string
        Dim queryString As String = "?UserID=" & userID & "&UserName=" & userName & "&Email=" & email

        ' Update links with query parameters
        dashboardLink.HRef = "UserDashboard.aspx" & queryString
        fillApplicationButton.HRef = "ApplicationForm.aspx" & queryString  ' Keep as it is since no navigation link is specified
        statusLink.HRef = "ApplicationStatus.aspx" & queryString
        forumLink.HRef = "DiscussionForum.aspx" & queryString
        previewLink.HRef = "ApplicationFormPreview.aspx" & queryString
        profileLink.HRef = "UserProfilePage.aspx" & queryString
        profileLink2.HRef = "UserProfilePage.aspx" & queryString
        changePass.HRef = "ChangePassword.aspx" & queryString
    End Sub


    Public Function CalculateStudentPointsPercentageApplication(ByVal UserID As String) As Double
        Dim totalPoints As Integer = 27
        Dim achievedPoints As Integer = 0

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check 23 fields in Sturegistration table
            Dim registrationFields As String() = {
                "firstName", "motherName", "Email", "GuardianMobile", "CourseID", "Religion", "gender", "seatid", "DOB",
                "FatherName", "RegistrationFee", "Padd", "PCountry", "Pcity", "Pstate", "ppincode", "Radd", "RCountry",
                "RCity", "Rstate", "Rpincode", "MobileNo", "GuardianIncome"
            }
            Dim query As String = "SELECT " & String.Join(",", registrationFields) & " FROM Sturegistration WHERE RegistrationID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        For Each field In registrationFields
                            If Not IsDBNull(reader(field)) AndAlso Not String.IsNullOrWhiteSpace(reader(field).ToString()) Then
                                achievedPoints += 1
                            End If
                        Next
                    End If
                End Using
            End Using

            ' Check StudentEducation table
            query = "SELECT COUNT(*) FROM StudentEducation WHERE StudentID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using

            ' Check StudentEssentialDoc table for 'photo'
            query = "SELECT COUNT(*) FROM StudentEssentialDoc WHERE StudentID = @UserID AND Docxpath LIKE '%photo%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using

            ' Check StudentEssentialDoc table for 'sign'
            query = "SELECT COUNT(*) FROM StudentEssentialDoc WHERE StudentID = @UserID AND Docxpath LIKE '%sign%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using

            ' Check StudentEssentialDoc table for other documents
            query = "SELECT COUNT(*) FROM StudentEssentialDoc WHERE StudentID = @UserID AND Docxpath NOT LIKE '%photo%' AND Docxpath NOT LIKE '%sign%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using
        End Using

        ' Calculate and return percentage
        Return (achievedPoints / totalPoints) * 100
    End Function
    Public Function CalculateStudentPointsPercentageUserProfile(ByVal UserID As String) As Double
        Dim totalPoints As Integer = 25
        Dim achievedPoints As Integer = 0

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check 23 fields in Sturegistration table
            Dim registrationFields As String() = {
                "firstName", "motherName", "Email", "GuardianMobile", "CourseID", "Religion", "gender", "seatid", "DOB",
                "FatherName", "Padd", "PCountry", "Pcity", "Pstate", "ppincode", "Radd", "RCountry",
                "RCity", "Rstate", "Rpincode", "MobileNo", "GuardianIncome"
            }
            Dim query As String = "SELECT " & String.Join(",", registrationFields) & " FROM Sturegistration WHERE RegistrationID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        For Each field In registrationFields
                            If Not IsDBNull(reader(field)) AndAlso Not String.IsNullOrWhiteSpace(reader(field).ToString()) Then
                                achievedPoints += 1
                            End If
                        Next
                    End If
                End Using
            End Using

            ' Check StudentEducation table
            query = "SELECT COUNT(*) FROM StudentEducation WHERE StudentID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using

            ' Check StudentEssentialDoc table for 'photo'
            query = "SELECT COUNT(*) FROM StudentEssentialDoc WHERE StudentID = @UserID AND Docxpath LIKE '%photo%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using

            ' Check StudentEssentialDoc table for 'sign'
            query = "SELECT COUNT(*) FROM StudentEssentialDoc WHERE StudentID = @UserID AND Docxpath LIKE '%sign%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", UserID)
                If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                    achievedPoints += 1
                End If
            End Using

            ' Check StudentEssentialDoc table for other documents
            'query = "SELECT COUNT(*) FROM StudentEssentialDoc WHERE StudentID = @UserID AND Docxpath NOT LIKE '%photo%' AND Docxpath NOT LIKE '%sign%'"
            'Using cmd As New SqlCommand(query, conn)
            '    cmd.Parameters.AddWithValue("@UserID", UserID)
            '    If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
            '        achievedPoints += 1
            '    End If
            'End Using
        End Using

        ' Calculate and return percentage
        Return (achievedPoints / totalPoints) * 100
    End Function
    Public Sub PopulateProfileDetails()
        ' Ensure RegistrationID is provided in the query string
        If Request.QueryString("UserID") Is Nothing OrElse String.IsNullOrEmpty(Request.QueryString("UserID").ToString()) Then
            Throw New Exception("No RegistrationID found in the query string.")
        End If

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT stu.Student AS FullName, stu.Email, stu.MobileNo, c.Course AS CourseName " & _
                                  "FROM stuRegistration stu " & _
                                  "JOIN Exam_course c ON stu.Courseid = c.courseid " & _
                                  "WHERE stu.RegistrationID = @RegistrationID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@RegistrationID", Request.QueryString("UserID"))

                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Assign values to the labels
                        FullnameP.Text = reader("FullName").ToString()
                        emailP.Text = reader("Email").ToString()
                        mobileP.Text = reader("MobileNo").ToString()
                        courseP.Text = reader("CourseName").ToString()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Public Function GetRegistrationIDByEmail() As Integer
        ' Ensure the session has a valid email value
        ' Initialize the connection and SQL command
        Dim registrationID As Integer = 0
        Dim query As String = "SELECT RegistrationID FROM stuRegistration WHERE Email = @Email"

        ' Create connection object
        Using con As New SqlConnection(connStr)
            ' Create the SQL command
            Using cmd As New SqlCommand(query, con)
                ' Add parameter for email
                cmd.Parameters.AddWithValue("@Email", Request.QueryString("Email").ToString())

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

    Public Function GetRemarkByUserID() As String
        Dim remark As String = String.Empty
        Dim email As String = Request.QueryString("Email").ToString() ' Get the Email from the session
        Dim query As String = "SELECT Remark FROM stuRegistration WHERE Email = @Email"

        ' Using block ensures the connection is properly disposed after use
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                ' Add parameter for the Email (from session)
                cmd.Parameters.AddWithValue("@Email", email)

                Try
                    conn.Open()

                    ' Execute the query and get the Remark
                    Dim result As Object = cmd.ExecuteScalar()

                    ' If result is not null, assign it to the remark variable
                    If result IsNot Nothing Then
                        remark = result.ToString()
                    End If
                Catch ex As Exception
                    ' Handle any potential exceptions
                    ' Log the exception (for debugging purposes)
                    Throw New Exception("Error fetching remark: " & ex.Message)
                End Try
            End Using
        End Using

        Return remark

    End Function

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

    Protected Function GetProfileStatusPercentFromDatabase(ByVal registrationID As String) As Double
        Dim profilePercent As Double = 0

        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ProfileStatusPercent FROM stuRegistration WHERE RegistrationID = @RegistrationID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            If Not IsDBNull(reader("ProfileStatusPercent")) Then
                                profilePercent = Convert.ToDouble(reader("ProfileStatusPercent"))
                            End If
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' Log or handle the exception
            profilePercent = 0 ' Default value in case of an error
        End Try

        Return profilePercent
    End Function

    Protected Function GetApplicationFormPercentFromDatabase(ByVal registrationID As String) As Double
        Dim formPercent As Double = 0

        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ApplicationFormPercent FROM stuRegistration WHERE RegistrationID = @RegistrationID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            If Not IsDBNull(reader("ApplicationFormPercent")) Then
                                formPercent = Convert.ToDouble(reader("ApplicationFormPercent"))
                            End If
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' Log or handle the exception
            formPercent = 0 ' Default value in case of an error
        End Try

        Return formPercent
    End Function


    Protected Sub EditProfileBtn_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx")
    End Sub
End Class
