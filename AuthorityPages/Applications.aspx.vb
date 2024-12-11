Imports System.Data.SqlClient
Imports System.Data

Partial Class AuthorityPages_Applications
    Inherits System.Web.UI.Page
    Dim connString As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadCourseData("All") ' Load all courses with default filter
        End If
    End Sub

    ' Dropdown filter change event
    Protected Sub ddlStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim filter As String = ddlStatusFilter.SelectedValue
        LoadCourseData(filter)
    End Sub
    ' Load course data based on the selected filter
    Private Sub LoadCourseData(ByVal filter As String)
        Using conn As New SqlConnection(connString)
            Dim query As String = "SELECT ec.Course, ec.Courseid, ec.coursecode, " & _
                                  "ISNULL(SUM(CASE " & _
                                  "   WHEN sr.ApplicationStatus IN ('AppSubmitted', 'Verified', 'Pending', 'Approved', 'Rejected', 'DocVerified') THEN 1 " & _
                                  "   ELSE 0 END), 0) AS NoOfApplicants " & _
                                  "FROM Exam_course ec " & _
                                  "LEFT JOIN sturegistration sr ON ec.Courseid = sr.courseid "

            ' Apply filter based on dropdown selection
            If filter <> "All" Then
                query &= "WHERE sr.ApplicationStatus = @Filter "
            End If

            ' Group by course details
            query &= "GROUP BY ec.Course, ec.Courseid, ec.coursecode"

            ' Execute the query and bind the result to the GridView
            Using cmd As New SqlCommand(query, conn)
                If filter <> "All" Then
                    cmd.Parameters.AddWithValue("@Filter", filter)
                End If

                conn.Open()
                Dim dt As New DataTable()
                Using sda As New SqlDataAdapter(cmd)
                    sda.Fill(dt)
                End Using
                GridViewCourses.DataSource = dt
                GridViewCourses.DataBind()
            End Using
        End Using
    End Sub

    Protected Sub backbotton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles backbotton.Click
        Response.Redirect("AdminDashboard.aspx")
    End Sub
    ' Handle RowDataBound event for conditional formatting
    Protected Sub GridViewCourses_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridViewCourses.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim applicantsCount As Integer = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "NoOfApplicants"))
            Dim lblApplicants As Label = CType(e.Row.FindControl("lblApplicants"), Label)

            '' Set color based on number of applicants
            'If applicantsCount = 0 Then
            '    lblApplicants.ForeColor = System.Drawing.Color.Red
            'Else
            '    lblApplicants.ForeColor = System.Drawing.Color.Black
            'End If
        End If
    End Sub
End Class
