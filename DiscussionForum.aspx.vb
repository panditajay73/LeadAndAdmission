Imports System.Data.SqlClient
Imports System.IO
Imports System.Data

Partial Class DiscussionForum
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Check if user is logged in

        If Not IsPostBack Then
            ' Check if the user is logged in
            If Request.QueryString("UserID") Is Nothing OrElse Request.QueryString("Email") Is Nothing OrElse Request.QueryString("UserName") Is Nothing Then
                ' If not logged in, redirect to the login page
                Response.Redirect("Login.aspx")
            End If
            Dim userId As String = Request.QueryString("UserID")
            userIcon.ImageUrl = GetUserPhoto(userId)

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

            BindQuestions()
            PopulateCategories()
        End If
    End Sub
    Private Sub PopulateCategories()
        ' Clear any existing items
        ddlCategory.Items.Clear()

        ' Add the default "Select Category" item
        ddlCategory.Items.Add(New ListItem("Select Category", "0"))

        ' Add hardcoded categories
        Dim categories As List(Of String) = New List(Of String) From {
            "Admission Queries",
            "Fee Structure",
            "Scholarship Opportunities",
            "Course Details",
            "Exam Schedules",
            "Result Updates",
            "Hostel Facilities",
            "Transport Services",
            "Library Access",
            "Extracurricular Activities"
        }

        For Each category As String In categories
            ddlCategory.Items.Add(New ListItem(category, category))
        Next
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Validate dropdown and textarea inputs
        If ddlCategory.SelectedIndex = 0 Then
            Response.Write("Please select a valid category.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtQuestion.Value) Then
            Response.Write("Please enter a question.")
            Exit Sub
        End If

        Try
            Dim userId As String = Request.QueryString("UserID") ' Assuming UserID is passed as a query string
            Dim categoryText As String = ddlCategory.SelectedItem.Text
            Dim questionText As String = txtQuestion.Value

            Using conn As New SqlConnection(connStr)
                Dim query As String = "INSERT INTO Topics (Title, Content, CreatedBy, CreatedDate) VALUES (@Title, @Content, @CreatedBy, GETDATE())"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Title", categoryText)
                    cmd.Parameters.AddWithValue("@Content", questionText)
                    cmd.Parameters.AddWithValue("@CreatedBy", userId)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Clear inputs and rebind questions
            ddlCategory.SelectedIndex = 0
            txtQuestion.Value = ""
            BindQuestions()
            Response.Write("Your question has been submitted successfully.")
        Catch ex As Exception
            ' Handle exceptions
            Response.Write("An error occurred: " & ex.Message)
        End Try
    End Sub


    Private Sub BindQuestions()
        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT TopicID, Title, Content, CreatedBy, CONVERT(VARCHAR(10), CreatedDate, 120) AS CreatedDate, RIGHT(CONVERT(VARCHAR(20), CreatedDate, 100), 7) AS CreatedTime FROM Topics ORDER BY CreatedDate DESC"
                Using cmd As New SqlCommand(query, conn)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim dt As New DataTable()
                        dt.Load(reader)

                        ' Bind data to Repeater
                        rptQuestions.DataSource = dt
                        rptQuestions.DataBind()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Handle exception
            Response.Write("An error occurred while loading questions: " & ex.Message)
        End Try
    End Sub

    Protected Sub rptQuestions_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            ' Get the UserID from the query string
            Dim currentUserId As String = Request.QueryString("UserID")

            ' Get the CreatedBy value from the data item
            Dim createdBy As String = DataBinder.Eval(e.Item.DataItem, "CreatedBy").ToString()

            ' Find the delete button in the current Repeater item
            Dim deleteButton As HtmlGenericControl = CType(e.Item.FindControl("deleteButton"), HtmlGenericControl)

            ' Show or hide the delete button based on the condition
            If createdBy = currentUserId Then
                deleteButton.Visible = True
            Else
                deleteButton.Visible = False
            End If
        End If
    End Sub


    Protected Function GetUserPhoto(ByVal userID As String) As String
        Dim defaultPhoto As String = "~/images/user.png"
        Dim photoPath As String = defaultPhoto

        Try
            Using conn As New SqlConnection(connStr)
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

    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserDashboard.aspx?UserID=" & Request.QueryString("UserID") & "&UserName=" & Request.QueryString("UserName") & "&Email=" & Request.QueryString("Email"))
    End Sub
End Class
