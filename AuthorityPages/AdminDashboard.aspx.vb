Imports System.Data.SqlClient
Imports System.Configuration
Partial Class AuthorityPages_AdminDashboard
    Inherits System.Web.UI.Page

    Dim connectionString As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Get all existing application and document counts
            Dim totalApplications As Integer = GetTotalApplications()
            Dim pendingApplications As Integer = GetPendingApplications()
            Dim approvedApplications As Integer = GetApprovedApplications()
            Dim verifiedApplications As Integer = GetVerifiedApplications()
            Dim docVerifiedApplications As Integer = GetDocVerifiedApplications()
            Dim rejectedApplications As Integer = GetRejectedApplications()
            Dim totalCourseOpen As Integer = GetTotalCourses()
            Dim statusCounts As Dictionary(Of String, Integer) = GetDocumentVerificationStatusCounts()

            ' Fill in the frontend dynamically
            UpdateFrontend(totalApplications, pendingApplications, approvedApplications, rejectedApplications, statusCounts)

            ' Existing Chart scripts and functionality
            totalApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & totalApplications.ToString() & "</h1>"
            'pendingApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & pendingApplications.ToString() & "</h1>"
            approvedApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & approvedApplications.ToString() & "</h1>"
            verifiedApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & docVerifiedApplications.ToString() & "</h1>"
            docVerifiedApplicationsDiv.InnerHtml = "<h1 style='text-align:center'>" & verifiedApplications.ToString() & "</h1>"
            totalCourses.InnerHtml = "<h1 style='text-align:center'>" & totalCourseOpen.ToString() & "</h1>"

            Dim verifiedCount As Integer = statusCounts("Verified")
            Dim rejectedCount As Integer = statusCounts("Rejected")
            Dim pendingCount As Integer = statusCounts("Pending")
            'Dim approvedCount As Integer = statusCounts("Approved")

            Dim chartScript As String = "<script type='text/javascript'>" &
                                         "var verifiedCount = " & verifiedCount & ";" &
                                         "var rejectedCount = " & rejectedCount & ";" &
                                         "var pendingCount = " & pendingCount & ";" &
                                         "renderChart('pie');" &
                                         "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "chartScript", chartScript)
            Dim funnelData As String = GetAdmissionFunnelData()
            Dim funnelChartScript As String = "<script type='text/javascript'>" &
                                              "var chartData = " & funnelData & ";" &
                                              "renderFunnelChart('funnel');" &
                                              "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "RenderFunnelChart", funnelChartScript, False)
            'Dim leadManagementScript As String = PrepareChartScript()
            'ClientScript.RegisterStartupScript(Me.GetType(), "leadManagementChartScript", leadManagementScript)
            RenderApplicantDemographics()

            Dim programCounts As Dictionary(Of String, Integer) = GetProgramSpecificApplications()


            ' Pass the program data to the frontend
            RenderProgramSpecificApplications()
            BindGrid(5)
            BindGridddlItemCountLatest(5)
            RenderFeeStatus()
        End If
    End Sub
    Private Function GetAdmissionFunnelData() As String
        Dim query As String = " SELECT COUNT(CASE WHEN ApplicationStatus IS NOT NULL THEN 1 END) AS Applications, COUNT(CASE WHEN AdmissionFeeStatus = 'Submitted' THEN 1 END) AS Payment, COUNT(CASE WHEN ApplicationStatus = 'Approved' THEN 1 END) AS Admission FROM StuRegistration"

    Using con As New SqlConnection(connectionString)
        Using cmd As New SqlCommand(query, con)
            con.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                        ' Prepare JSON-like data for chart
                Dim applications As Integer = Convert.ToInt32(reader("Applications"))
                Dim payment As Integer = Convert.ToInt32(reader("Payment"))
                Dim admission As Integer = Convert.ToInt32(reader("Admission"))

                        ' Format data as a valid JavaScript array
                Return "[" & 
                    "{ y: " & applications & ", label: 'Applications' }," &
                    "{ y: " & payment & ", label: 'Payment' }," &
                    "{ y: " & admission & ", label: 'Admission' }" &
                "]"
                    End If
                End Using
            End Using
    Return "[]" ' Empty array if no data found
    End Function

    ' Method to bind data to the grid
    Private Sub BindGrid(ByVal count As Integer)
        Dim query As String = "SELECT TOP(@Count) ROW_NUMBER() OVER (ORDER BY Dated DESC) AS SrNo, Student As Name, Applycourse AS Program, Dated AS Date, Status FROM stuRegistration WHERE ApplicationStatus = 'AppSubmitted'"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Count", count)
                con.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    gvApplications.DataSource = reader
                    gvApplications.DataBind()
                    gvApplications.Visible = True
                    lblMessage.Visible = False
                Else
                    gvApplications.Visible = False
                    lblMessage.Text = "There is no application yet."
                    lblMessage.Visible = True
                End If
            End Using
        End Using
    End Sub

    Private Sub BindGridddlItemCountLatest(ByVal count As Integer)
        Dim query As String = "SELECT TOP(@Count) ROW_NUMBER() OVER (ORDER BY Dated DESC) AS SrNo, Student As Name, Applycourse AS Program, Dated AS Date, Status FROM stuRegistration where ApplicationStatus = 'RegistrationApproved' or ApplicationStatus = 'Approved'"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Count", count)
                con.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                gvApplications_ddlItemCountLatest.DataSource = reader
                gvApplications_ddlItemCountLatest.DataBind()
            End Using
        End Using
    End Sub
    Protected Sub ddlItemCountLatest_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Fetch the selected value from the dropdown and update the grid accordingly
        Dim selectedCount As Integer = Convert.ToInt32(ddlItemCountLatest.SelectedValue)
        BindGridddlItemCountLatest(selectedCount)
    End Sub
    ' Event handler for dropdown selection change
    Protected Sub ddlItemCount_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Fetch the selected value from the dropdown and update the grid accordingly
        Dim selectedCount As Integer = Convert.ToInt32(ddlItemCount.SelectedValue)
        BindGrid(selectedCount)
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
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration WHERE RegistrationApproved = '1' And ApplicationStatus='Verified'"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            verifiedCount = Convert.ToInt32(cmd.ExecuteScalar())
        End Using

        Return verifiedCount
    End Function
    Protected Function GetDocVerifiedApplications() As Integer
        Dim docVerifiedCount As Integer = 0

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration WHERE ApplicationStatus='DocVerified'"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            docVerifiedCount = Convert.ToInt32(cmd.ExecuteScalar())
        End Using

        Return docVerifiedCount
    End Function
    Private Sub RenderApplicationStages()
        Dim verifiedCount As Integer = GetVerifiedApplications() ' Get verified applications count
        Dim docverifiedCount As Integer = GetDocVerifiedApplications()
        Dim pendingCount As Integer = GetPendingApplications()
        Dim approvedCount As Integer = GetApprovedApplications()
        Dim rejectedCount As Integer = GetRejectedApplications()

        ' Construct the data points manually as a string
        Dim dataPoints As String = "[ " &
            "{ label: 'Pending', y: " & pendingCount & " }, " &
            "{ label: 'Approved', y: " & approvedCount & " }, " &
            "{ label: 'Rejected', y: " & rejectedCount & " }, " &
            "{ label: 'Verified', y: " & verifiedCount & " } " &
            "{ label: 'Document Verified', y: " & docverifiedCount & " } " &
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
            Dim query As String = "SELECT courseid FROM Exam_Course"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            ' Initialize program counts
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim coursePrefix As String = reader("courseid").ToString()
                    programCounts(coursePrefix) = 0 ' Initialize count to 0 for each program
                End While
            End Using

            ' Query to fetch application counts for each course
            query = "SELECT ec.course, COUNT(sr.courseid) AS ApplicationCount " &
        "FROM Exam_Course ec " &
        "JOIN stuRegistration sr ON ec.courseid = sr.courseid " &
        "WHERE sr.ApplicationStatus IS NOT NULL AND sr.ApplicationStatus <> '' " &
        "GROUP BY ec.course"


            cmd = New SqlCommand(query, conn)

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim coursePrefix As String = reader("course").ToString()
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
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration WHERE ApplicationStatus = 'Approved' AND AdmissionFeeStatus = 'Submitted'"
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
    Protected Function GetTotalCourses() As Integer
        Dim totalCourses As Integer = 0
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM Exam_course", conn)
            conn.Open()
            totalCourses = Convert.ToInt32(cmd.ExecuteScalar())
        End Using
        Return totalCourses
    End Function
    Protected Function GetPendingApplications() As Integer
        Dim pendingApplications As Integer = 0
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM stuRegistration " & _
                                  "WHERE ApplicationStatus = 'AppSubmitted' Or ApplicationStatus='Verified' Or ApplicationStatus='Pending'"
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()
            pendingApplications = Convert.ToInt32(cmd.ExecuteScalar())
        End Using
        Return pendingApplications
    End Function

    Private Sub InitializeCounts()
        Dim applicationFeePaidCount As Integer = 0
        Dim admissionFeePaidCount As Integer = 0
        Dim admissionFeeNotPaidCount As Integer = 0

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim command As New SqlCommand("SELECT FeeStatus, AdmissionFeeStatus FROM stuRegistration", connection)
            Dim reader As SqlDataReader = command.ExecuteReader()

            While reader.Read()
                Dim feeStatus As String = reader("FeeStatus").ToString()
                Dim admissionFeeStatus As String = reader("AdmissionFeeStatus").ToString()

                If feeStatus = "Submitted" Then
                    If admissionFeeStatus = "Submitted" Then
                        admissionFeePaidCount += 1
                    Else
                        applicationFeePaidCount += 1
                    End If
                ElseIf admissionFeeStatus = "Submitted" Then
                    admissionFeePaidCount += 1
                End If
            End While
        End Using

        ' Calculate the number of students who paid only the Application Fee
        admissionFeeNotPaidCount = applicationFeePaidCount - admissionFeePaidCount

        ' Prepare the JavaScript to expose counts to the frontend
        Dim chartScript As String = "<script type='text/javascript'>" & _
                                     "var admissionFeePaid = " & admissionFeePaidCount & ";" & _
                                     "var applicationFeePaid = " & applicationFeePaidCount & ";" & _
                                     "var admissionFeeNotPaid = " & admissionFeeNotPaidCount & ";" & _
                                     "renderFeeChart('pie');" & _
                                     "</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "chartScript", chartScript)
    End Sub
    Private Sub RenderFeeStatus()
        Dim applicationFeePaid As Integer = 0
        Dim admissionFeePaid As Integer = 0
        Dim admissionFeeNotPaid As Integer = 0

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT FeeStatus, AdmissionFeeStatus FROM stuRegistration WHERE FeeStatus IS NOT NULL"

        Dim cmd As New SqlCommand(query, conn)
        conn.Open()

        Using reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                Dim feeStatus As String = reader("FeeStatus").ToString()
                Dim admissionFeeStatus As String = reader("AdmissionFeeStatus").ToString()

                    If feeStatus = "Submitted" OrElse admissionFeeStatus = "Submitted" Then
                        applicationFeePaid += 1
                    End If

                    ' Admission Fee Submitted: both FeeStatus = "Submitted" AND AdmissionFeeStatus = "Submitted"
                    If feeStatus = "Submitted" AndAlso admissionFeeStatus = "Submitted" Then
                        admissionFeePaid += 1
                    End If

                    ' Admission Fee Not Submitted: FeeStatus = "Submitted" but AdmissionFeeStatus is NULL or empty
                    If feeStatus = "Submitted" AndAlso String.IsNullOrEmpty(admissionFeeStatus) Then
                        admissionFeeNotPaid += 1
                    End If
                    End While
                End Using
    End Using

        ' Exit if no data exists
    If applicationFeePaid = 0 And admissionFeePaid = 0 And admissionFeeNotPaid = 0 Then Exit Sub

        ' Register data points for JavaScript
    Dim script As String = "<script type='text/javascript'>" &
                           "var applicationFeePaid = " & applicationFeePaid & ";" &
                           "var admissionFeePaid = " & admissionFeePaid & ";" &
                           "var admissionFeeNotPaid = " & admissionFeeNotPaid & ";" &
                           "window.onload = function() { renderFeeChart('pie'); };" &
                           "</script>"

    ClientScript.RegisterStartupScript(Me.GetType(), "RenderFeeChart", script, False)
    End Sub

End Class
