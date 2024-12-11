Imports System.Data.SqlClient

Partial Class AuthorityPages_Dashboard
    Inherits System.Web.UI.Page

    Dim connectionString As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Get all existing application and document counts
            Dim totalApplications As Integer = GetTotalApplications()
            Dim pendingApplications As Integer = GetPendingApplications()
            Dim approvedApplications As Integer = GetApprovedApplications()
            Dim verifiedApplications As Integer = GetVerifiedApplications()

            Dim rejectedApplications As Integer = GetRejectedApplications()
            Dim statusCounts As Dictionary(Of String, Integer) = GetDocumentVerificationStatusCounts()

            ' Fill in the frontend dynamically
            UpdateFrontend(totalApplications, pendingApplications, approvedApplications, rejectedApplications, statusCounts)

            ' Existing Chart scripts and functionality
            totalApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & totalApplications.ToString() & "</h1>"
            pendingApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & pendingApplications.ToString() & "</h1>"
            verifiedApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & verifiedApplications.ToString() & "</h1>"
            approvedApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & approvedApplications.ToString() & "</h1>"
            rejectedApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & rejectedApplications.ToString() & "</h1>"

            Dim verifiedCount As Integer = statusCounts("Verified")
            Dim rejectedCount As Integer = statusCounts("Rejected")
            Dim pendingCount As Integer = statusCounts("Pending")
            Dim chartScript As String = "<script type='text/javascript'>" &
                                         "var verifiedCount = " & verifiedCount & ";" &
                                         "var rejectedCount = " & rejectedCount & ";" &
                                         "var pendingCount = " & pendingCount & ";" &
                                         "renderChart('pie');" &
                                         "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "chartScript", chartScript)

            'Dim leadManagementScript As String = PrepareChartScript()
            'ClientScript.RegisterStartupScript(Me.GetType(), "leadManagementChartScript", leadManagementScript)
            RenderApplicantDemographics()
            RenderApplicationStages()

            Dim programCounts As Dictionary(Of String, Integer) = GetProgramSpecificApplications()

            ' Pass the program data to the frontend
            RenderProgramSpecificApplications()
        End If
    End Sub
    Protected Sub RenderApplicantDemographics()
        Dim genderCounts As Dictionary(Of String, Integer) = GetGenderCounts()

        Dim maleCount As Integer = If(genderCounts.ContainsKey("Male"), genderCounts("Male"), 0)
        Dim femaleCount As Integer = If(genderCounts.ContainsKey("Female"), genderCounts("Female"), 0)
        Dim otherCount As Integer = If(genderCounts.ContainsKey("Other"), genderCounts("Other"), 0)

        ' Register JavaScript to pass the data to the chart rendering
        ClientScript.RegisterStartupScript(Me.GetType(), "applicantDemographicsScript",
            "<script type='text/javascript'>" &
            "var maleCount = " & maleCount & ";" &
            "var femaleCount = " & femaleCount & ";" &
            "var otherCount = " & otherCount & ";" &
            "</script>")
    End Sub

    Private Function GetGenderCounts() As Dictionary(Of String, Integer)
        Dim genderCounts As New Dictionary(Of String, Integer)

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT Gender, COUNT(*) AS Count FROM stuRegistration GROUP BY Gender"

            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim gender As String = reader("Gender").ToString()
                    Dim count As Integer = Convert.ToInt32(reader("Count"))

                    genderCounts(gender) = count
                End While
            End Using
        End Using

        Return genderCounts
    End Function
    Protected Function GetVerifiedApplications() As Integer
        Dim verifiedCount As Integer = 0

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration WHERE ApplicationStatus = 'Verified'"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            verifiedCount = Convert.ToInt32(cmd.ExecuteScalar())
        End Using

        Return verifiedCount
    End Function
    Private Sub RenderApplicationStages()
        Dim verifiedCount As Integer = GetVerifiedApplications() ' Get verified applications count
        Dim pendingCount As Integer = GetPendingApplications()
        Dim approvedCount As Integer = GetApprovedApplications()
        Dim rejectedCount As Integer = GetRejectedApplications()

        ' Construct the data points manually as a string
        Dim dataPoints As String = "[ " &
            "{ label: 'Pending', y: " & pendingCount & " }, " &
            "{ label: 'Approved', y: " & approvedCount & " }, " &
            "{ label: 'Rejected', y: " & rejectedCount & " }, " &
            "{ label: 'Verified', y: " & verifiedCount & " } " &
        "]"

        ' Register JavaScript for rendering the chart
        Dim chartScript As String = "<script type='text/javascript'>" &
                                     "renderAppChart('pie', " & dataPoints & ");" &
                                     "renderAppGrid(" & dataPoints & ");" &
                                     "</script>"

        ClientScript.RegisterStartupScript(Me.GetType(), "applicationStagesScript", chartScript)
    End Sub

    Private Sub UpdateFrontend(ByVal totalApplications As Integer, ByVal pendingApplications As Integer, ByVal approvedApplications As Integer, ByVal rejectedApplications As Integer, ByVal statusCounts As Dictionary(Of String, Integer))
        ' Prepare percentages for documents
        Dim totalDocuments As Integer = statusCounts("Verified") + statusCounts("Rejected") + statusCounts("Pending")
        Dim verifiedPercentage As Double = If(totalDocuments > 0, (statusCounts("Verified") / totalDocuments) * 100, 0)
        Dim rejectedPercentage As Double = If(totalDocuments > 0, (statusCounts("Rejected") / totalDocuments) * 100, 0)
        Dim pendingPercentage As Double = If(totalDocuments > 0, (statusCounts("Pending") / totalDocuments) * 100, 0)

        ' Update document summary frontend
        verifiedDocumentsDiv.InnerHtml = "<span class='badge bg-success text-white mb-2'><i class='fas fa-file-alt'></i> Verified Documents (" &
                                         statusCounts("Verified") & "): " & verifiedPercentage.ToString("0.0") & "%</span>"
        pendingDocumentsDiv.InnerHtml = "<span class='badge bg-secondary text-white mb-2'><i class='fas fa-hourglass-half'></i> Pending Documents (" &
                                        statusCounts("Pending") & "): " & pendingPercentage.ToString("0.0") & "%</span>"
        rejectedDocumentsDiv.InnerHtml = "<span class='badge bg-danger text-white mb-2'><i class='fas fa-times-circle'></i> Rejected Documents (" &
                                         statusCounts("Rejected") & "): " & rejectedPercentage.ToString("0.0") & "%</span>"

        ' Prepare percentages for applications
        Dim totalApp As Integer = totalApplications
        Dim approvedPercentage As Double = If(totalApp > 0, (approvedApplications / totalApp) * 100, 0)
        Dim rejectedAppPercentage As Double = If(totalApp > 0, (rejectedApplications / totalApp) * 100, 0)
        Dim pendingAppPercentage As Double = If(totalApp > 0, (pendingApplications / totalApp) * 100, 0)

        ' Update application summary frontend
        approvedApplicationsDiv2.InnerHtml = "<span class='badge bg-success text-white mb-2'><i class='fas fa-check'></i> Approved Applications (" &
                                            approvedApplications & "): " & approvedPercentage.ToString("0.0") & "%</span>"
        rejectedApplicationsDiv2.InnerHtml = "<span class='badge bg-danger text-white mb-2'><i class='fas fa-times'></i> Rejected Applications (" &
                                            rejectedApplications & "): " & rejectedAppPercentage.ToString("0.0") & "%</span>"
        pendingApplicationsDiv2.InnerHtml = "<span class='badge bg-secondary text-white mb-2'><i class='fas fa-clock'></i> Pending Applications (" &
                                           pendingApplications & "): " & pendingAppPercentage.ToString("0.0") & "%</span>"
    End Sub

    Private Sub RenderProgramSpecificApplications()
        Dim programCounts As New Dictionary(Of String, Integer)

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT ec.course, COUNT(sr.courseid) AS ApplicationCount " &
                                  "FROM Exam_Course ec " &
                                  "LEFT JOIN stuRegistration sr ON ec.courseid = sr.courseid " &
                                  "WHERE sr.ApplicationStatus IS NOT NULL AND sr.ApplicationStatus <> '' " &
                                  "GROUP BY ec.course"

            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim course As String = reader("course").ToString()
                    Dim count As Integer = Convert.ToInt32(reader("ApplicationCount"))
                    If count > 0 Then
                        programCounts(course) = count
                    End If
                End While
            End Using
        End Using

        ' Exit if no valid programs are found
        If programCounts.Count = 0 Then Exit Sub

        ' Prepare data points for chart rendering
        Dim dataPoints As String = String.Join(",", programCounts.Select(Function(kvp) "{ label: '" & kvp.Key & "', y: " & kvp.Value & " }"))

        ' Inject JavaScript to render the chart dynamically
        Dim script As String = "<script type='text/javascript'>" &
                               "var programDataPoints = [" & dataPoints & "];" &
                               "renderProgramChart('column');" &
                               "</script>"

        ClientScript.RegisterStartupScript(Me.GetType(), "RenderProgramChartScript", script, False)
    End Sub

    ' Method to fetch program-specific applications from the database
    Private Function GetProgramSpecificApplications() As Dictionary(Of String, Integer)
        Dim programCounts As New Dictionary(Of String, Integer)

        Using conn As New SqlConnection(connectionString)
            ' Fetch all available courses
            Dim query As String = "SELECT Coursecode FROM Exam_Course"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            ' Initialize program counts
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim coursePrefix As String = reader("Coursecode").ToString()
                    programCounts(coursePrefix) = 0 ' Initialize count to 0 for each program
                End While
            End Using

            ' Query to fetch application counts for each course
            query = "SELECT ec.Coursecode, COUNT(sr.ApplyCourse) AS ApplicationCount " &
                    "FROM Exam_Course ec " &
                    "LEFT JOIN stuRegistration sr ON ec.Coursecode = sr.ApplyCourse " &
                    "WHERE sr.ApplicationStatus IS NOT NULL AND sr.ApplicationStatus <> '' " &
                    "GROUP BY ec.Coursecode"

            cmd = New SqlCommand(query, conn)

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim coursePrefix As String = reader("Coursecode").ToString()
                    Dim count As Integer = Convert.ToInt32(reader("ApplicationCount"))
                    programCounts(coursePrefix) = count
                End While
            End Using
        End Using

        Return programCounts
    End Function

    Private Function GetDocumentVerificationStatusCounts() As Dictionary(Of String, Integer)
        Dim statusCounts As New Dictionary(Of String, Integer) From {
            {"Verified", 0},
            {"Rejected", 0},
            {"Pending", 0}
        }

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT ApplicationStatus, COUNT(*) AS StatusCount " &
                                  "FROM stuRegistration " &
                                  "WHERE ApplicationStatus IN ('Verified', 'Rejected', 'Pending', 'Approved', 'AppSubmitted', 'DocVerified') " &
                                  "GROUP BY ApplicationStatus"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim status As String = reader("ApplicationStatus").ToString()
                    Dim count As Integer = Convert.ToInt32(reader("StatusCount"))

                    Select Case status
                        Case "Verified", "Approved", "DocVerified"
                            statusCounts("Verified") += count
                        Case "Rejected"
                            statusCounts("Rejected") += count
                        Case "Pending", "AppSubmitted"
                            statusCounts("Pending") += count
                    End Select
                End While
            End Using
        End Using

        Return statusCounts
    End Function

    Protected Function GetRejectedApplications() As Integer
        Dim rejectedApplications As Integer = 0
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration " &
                                  "WHERE ApplicationStatus LIKE '%Reject%' " 
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            rejectedApplications = Convert.ToInt32(cmd.ExecuteScalar())
        End Using
        Return rejectedApplications
    End Function

    Protected Function GetApprovedApplications() As Integer
        Dim approvedApplications As Integer = 0
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration WHERE ApplicationStatus = 'Approved'"
            Dim cmd As New SqlCommand(query, conn)

            conn.Open()
            approvedApplications = Convert.ToInt32(cmd.ExecuteScalar())
        End Using
        Return approvedApplications
    End Function

    Protected Function GetTotalApplications() As Integer
        Dim totalApplications As Integer = 0
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM stuRegistration WHERE ApplicationStatus IS NOT NULL AND ApplicationStatus <> ''", conn)
            conn.Open()
            totalApplications = Convert.ToInt32(cmd.ExecuteScalar())
        End Using
        Return totalApplications
    End Function

Protected Function GetPendingApplications() As Integer
        Dim pendingApplications As Integer = 0
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration " & _
                                  "WHERE (ApplicationStatus = 'AppSubmitted' " & _
                                  " AND " & _
                                  "(AdmissionFeeStatus IS NULL OR AdmissionFeeStatus = ''))"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            pendingApplications = Convert.ToInt32(cmd.ExecuteScalar())
        End Using
        Return pendingApplications
    End Function


End Class
