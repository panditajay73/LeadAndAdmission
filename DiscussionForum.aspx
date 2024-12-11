<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DiscussionForum.aspx.vb" Inherits="DiscussionForum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Discussion Forum</title>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="Styles/UserDashboard.css">
    <style>
    div.main-container
{
    display: flex;
}
.container-left
{
    width: 40%;  
    height: 100px;
    display: flex;
    flex-direction: column; 
    background: #fff;
    min-height: 93.4vh;
    box-shadow: 1px 0 1px 0 #ccc;
    z-index: 3;
}
.container-right
{
    width: 60%;
    height: 100px;
    min-height: 93.4vh;
    color: #000;
    padding: 40px 60px 40px 80px;
    overflow-y: scroll;
}
.button-wrapper
{
    background: #fff;
    padding: 20px 10px;
    box-shadow: 0px 1px 1px 0 #ccc;
}
.btn-add
{
    background: #1ed085; /* Teal */
    padding: 8px 12px;
    font-size: 18px;
    color: #fff;
    outline: none;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}
.search
{
    outline: none;
    font-size: 18px;
    border-radius: 5px;
    border: 1px solid gray;
    padding: 8px 12px;
    margin-left: 10px;
}
#welcome
{
    margin-top:20px;
    display: block;
}
#welcome h1
{
    font-size: 3em;
}
.text
{
    color: gray;
    font-weight: bold;
}

form input
{
    width:40%;
    outline: none;
    font-size: 18px;
    border-radius: 5px;
    border: 1px solid #ddd;
    padding: 8px 12px;
    margin-top: 20px;
    color: gray;
}
input
{
    max-width: 100%;
}
form textarea
{
    width: 100%;
    outline: none;
    font-size: 18px;
    border-radius: 5px;
    border: 1px solid #ddd;
    padding: 8px 12px;
    margin-top: 15px;
    color: gray;
}
.btn-submit
{
    width: 100%;
    text-align: right;
}
.btn-submit button
{
    font-size:20px;
    padding: 8px 12px;  
    margin-top: 10px;
    border-radius: 5px;
    background: #009688; /* Teal */
    color: #fff;
    outline: none;
    border: none;
    cursor: pointer;
}
.btn-submit button:hover
{
    background: #00796b; /* Darker teal */
}
.questions
{
    overflow-y: scroll;
}
.que-item
{
    padding: 15px;
    color: #4f4f4f;
    font-weight: bold;
    border-bottom: 1px solid #ccc;
    cursor:pointer;
}
#description
{
    display: none;
}
.que-content
{
    background: #e0f7f7;
    padding: 10px;
    color: #3f3f3f;
    margin-top:5px;
}
.que-content h2
{
    font-size:1.3em;
}
p.que-text
{
    margin: 5px 0;
}
.resolve
{
    width: 100%;
    display: flex;
    justify-content: space-between;
    margin-top:10px;
}
.resolve button
{
    background: #008080; /* Teal */
    padding: 8px 12px;
    color: #fff;
    outline: none;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}
.responses
{
    background: #e0f7f7;
    margin-top:18px;
}
.response
{
    padding: 15px;
    color: #4f4f4f;
    border-bottom: 1px solid #ccc;
}
.response-form
{
    margin-top:40px;
}
.response-form form input
{
    font-size: 16px;
    padding: 6px 10px;
    margin-top: 15px;
}

.response-form form textarea
{
    font-size: 16px;
    padding: 6px 10px;
    margin-top: 10px;
}
.response-form .btn-submit button
{
    font-size:14px; 
    margin-top: 10px;
    background: #008080; /* Teal */
    padding: 8px 12px;
}
.yellow
{
    background: yellow;
}
.icons i
{
    padding: 0 3px;
}
.fa
{
    color: #000;
}
.fa:hover, .fa:active
{
    color: #008080; /* Teal */
}
i.active
{
    color: #008080; /* Teal */
}
.que-item .que-title, .que-item .que-text
{
    padding-right: 30px;
}
.que-item .fav
{
    float: right;
    position: relative;
    bottom: 34px;    
}

    </style>

    <style>
    .blurred-screen {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        backdrop-filter: blur(5px);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 999;
        color: white;
        text-align: center;
    }

    .blurred-message h1 {
        font-size: 2rem;
        margin-bottom: 10px;
    }

    .blurred-message p {
        font-size: 1.2rem;
    }
      .btn-home {
        margin-top: 20px;
        padding: 10px 20px;
        background-color: #1ed085;
        color: white;
        border: none;
        border-radius: 5px;
        font-size: 1rem;
        cursor: pointer;
        width:20%;
        transition: background-color 0.3s ease;
    }

    .btn-home:hover {
        background-color: #1ed085;
    }
.que-item {
    padding: 20px;
    border: 1px solid #e0e0e0;
    margin-bottom: 15px;
    border-radius: 8px;
    background-color: #ffffff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    transition: box-shadow 0.3s ease;
}

.que-item:hover {
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.que-title {
    font-size: 1.4em;
    font-weight: 600;
    color: #333333;
    margin: 0 0 10px;
}

.que-text {
    font-size: 1em;
    color: #555555;
    margin: 0 0 15px;
    line-height: 1.5;
}

.post-date, .post-time {
    font-size: 0.85em;
    color: #999999;
    font-weight: 500;
    text-align: right;
    margin: 0;

}

.post-info {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
}
/* Hide scrollbar for Blink-based browsers (Chrome, Edge, Opera, Safari) */
.questions::-webkit-scrollbar {
  display: none;
}

/* Hide scrollbar for Firefox */
html {
  scrollbar-width: none;
}

/* Hide scrollbar for Internet Explorer and Edge */
.questions {
  -ms-overflow-style: none;
}

</style>

<%--<script>
    // Simulate loader and show blurred section
    window.onload = function () {
        setTimeout(function () {
            // Hide loader
            document.getElementById("loader").style.display = "none";
            // Show blurred section
            document.getElementById("blurredSection").style.display = "flex";
        }, 2000); // Adjust loader duration as needed
    };
</script>--%>
   <%-- <script type="text/javascript">
        // Simulate a load time and hide the loader
        window.onload = function () {
            setTimeout(function () {
                document.getElementById("loader").style.display = "none";
                document.getElementById("content").style.display = "block";
            }, 2000); // Loader will display for 2 seconds
        };
    </script>--%>
</head>
<body>
   <form id="form1" runat="server">
<%--<div id="loader" class="loader-overlay">
        <div class="loader">
            <div class="dot"></div>
            <div class="dot"></div>
            <div class="dot"></div>
            <div class="dot"></div>
        </div>
    </div>--%>
        <!-- Top bar -->
    <div class="top-bar">
        <div class="logo-section">
            <img src="images/saralicon.png" alt="Logo" />
        </div>
         <div class="welcome-section">
        <% If Request.QueryString("UserName") IsNot Nothing Then %>
            <span>Welcome, <%= Request.QueryString("UserName")%>!</span>
        <% Else %>
            <span>Welcome, Guest!</span>
        <% End If %>

        <div class="user-menu">
            <asp:Image ID="userIcon" runat="server" ImageUrl="~/images/user.png" AlternateText="User Icon" CssClass="user-icon" />
            <div class="dropdown-content" id="dropdown-menu">
                <a href="#" id="profileLink2" runat="server"><i class="fa-solid fa-user"></i> &nbsp;Profile</a>
                <a href="#" ><i class="fa-solid fa-video"></i>&nbsp;Video Guide</a>
                <a href="#" id="changePass" runat="server"><i class="fa-solid fa-pen"></i> &nbsp;Change Password</a>
                <a href="Logout.aspx"><i class="fa-solid fa-right-from-bracket"></i> &nbsp;Logout</a>
            </div>
        </div>
    </div>
    </div>
      <!-- Navigation bar -->
    <div class="navbar">
        <a href="#" class="active" id="dashboardLink" runat="server"><i class="fa-solid fa-house"></i>Dashboard</a>
<a href="#" id="instructionsLink" runat="server"><i class="fa-solid fa-chalkboard-user"></i>Instructions</a>
<a href="#" id="statusLink" runat="server"><i class="fa-solid fa-bars-progress"></i>Status</a>
<a href="#" id="forumLink" runat="server"><i class="fa-solid fa-comment"></i>Discussion Forum</a>
<a href="#" id="previewLink" runat="server"><i class="fa-solid fa-magnifying-glass"></i>Preview</a>
<a href="#" id="profileLink" runat="server"><i class="fa-solid fa-user"></i>Profile</a>
<a href="#" id="fillApplicationButton" runat="server">
    <div class="fillApplication-button"><i class="fa-solid fa-fill"></i>&nbsp;Fill Application Form</div>
</a>
    </div>
    <!-- Blurred Screen Section -->
  <%--  <div id="blurredSection" class="blurred-screen" style="display: none;">
        <div class="blurred-message">
            <h1>Discussion Forum Module is Under Processing</h1>
            <p>Please check out another section!</p>
            <asp:Button ID="btnHome" runat="server" Text="Back Home" CssClass="btn-home" OnClick="btnRegister_Click"/>
        </div>
    </div>--%>

  
    <!-- Discussion Forum Content -->
    <div class="main-container">
       <div id="Div1" class="container-left" runat="server" visible="True" style="padding:10px 30px;">
   <div class="button-wrapper">
        <button class="btn-add" id="show-welcome">New Question Form</button>
        <input type="text" name="search" id="search" class="search" placeholder="Search questions . . ." />
        <button class="btn-add" id="show-favorites" style="margin-left: 10px">Favorites</button>
    </div>
    <div class="questions" style="display: flex; flex-direction: column; gap:5px;">
        <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
    <ItemTemplate>
        <div class="que-item" id='<%# Eval("TopicID") %>' style="position: relative; background: #f9f9f9; padding: 20px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);">
            <!-- Top Icons -->
            <div style="position: absolute; top: 10px; right: 10px; display: flex; flex-direction: column; gap: 10px;">
                <!-- Delete Icon -->
                <i class="fa fa-trash" 
                   title="Delete"
                   style="cursor: pointer; color: #e74c3c; font-size: 20px;"
                   runat="server" id="deleteButton">
                </i>
                <!-- Heart Icon -->
                <i class="fa fa-heart-o" 
                   title="Mark as Favorite"
                   onclick="toggleFavorite('<%# Eval("TopicID") %>')"
                   style="cursor: pointer; color: #ff4757; font-size: 20px;">
                </i>
            </div>
            
            <!-- Question Content -->
            <h2 class="que-title" style="font-size: 1.5em; color: #333; margin-bottom: 10px;"><%# Eval("Title") %></h2>
            <p class="que-text" style="font-size: 1em; color: #555; line-height: 1.5; margin-bottom: 20px;">
                <%# Eval("Content") %>
            </p>

            <!-- Footer with Date and Time -->
            <div style="display: flex; justify-content: flex-end; align-items: center; gap: 10px; font-size: 0.9em; color: #888;">
                <asp:Label ID="lblDate" runat="server" CssClass="post-date" 
                           Text='<%# Eval("CreatedDate") %>'>
                </asp:Label>
                <asp:Label ID="lblTime" runat="server" CssClass="post-time" 
                           Text='<%# Eval("CreatedTime") %>'>
                </asp:Label>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>

    </div>
</div>


        <div class="container-right" id="right" >

             <div id="welcome" runat="server" visible="True" 
     style="max-width: 800px; margin: 20px auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); background-color: #ffffff;">
    <h1 style="text-align: center; font-size: 24px; color: #333;">Welcome to the Discussion Portal!</h1>
    <p style="text-align: center; font-size: 16px; color: #555; margin-bottom: 20px;">Select a category and enter a question to get started:</p>
    <div id="formAdd" style="display: flex; flex-direction: column; gap: 15px;">
        <!-- Dropdown -->
        <asp:DropDownList ID="ddlCategory" runat="server" 
                          style="width: 100%; padding: 10px; font-size: 16px; border: 1px solid #ddd; border-radius: 5px; background-color: #f9f9f9; color: #333;">
            <asp:ListItem Text="Select Category" Value="0"></asp:ListItem>
        </asp:DropDownList>
        
        <!-- Textarea -->
        <textarea id="txtQuestion" runat="server" rows="6" 
                  placeholder="Write your question here..." 
                  style="width: 100%; padding: 10px; font-size: 16px; border: 1px solid #ddd; border-radius: 5px; background-color: #f9f9f9; color: #333; resize: none;"></textarea>
        
        <!-- Button -->
        <div class="btn-submit" style="text-align: center;">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                        style="padding: 10px 20px; font-size: 16px; border: none; border-radius: 5px; background-color: #1ed085; color: #fff; cursor: pointer; transition: background-color 0.3s;width:30%;" OnClick="btnSubmit_Click"/>
        </div>
    </div>
</div>


            <div id="description" runat="server" visible="False">
                <h2 class="text">Question</h2>
                <div class="que-content">
                    <h2 class="que-title" id="que-title">Web Development</h2>
                    <p class="que-text" id="que-text">What is Web?</p>
                    <span class="fav">Fav</span>
                </div>
                <div class="resolve">
                    <div class="icons">
                        <i class="fa fa-thumbs-up" id="upvote" aria-hidden="true"></i>
                        <span id="votes"></span>
                        <i class="fa fa-thumbs-down" id="downvote" aria-hidden="true"></i>	
                        <i class="fa fa-heart" id="favorite" aria-hidden="true"></i>
                    </div>
                    <button id="resolve">Resolve</button>
                </div>

                <div class="response-wrapper">
                    <h3 class="text">Response</h3>
                    <div class="responses" id="responses">
                        <div class="response">
                            <h3 class="res-name">Amit</h3>
                            <p class="res-text">Local storage can only save strings, so storing objects requires turning them into strings using JSON.</p>
                        </div>
                        <div class="response">
                            <h3 class="res-name">Amit</h3>
                            <p class="res-text">Local storage can only save strings, so storing objects requires turning them into strings using JSON.</p>
                        </div>
                    </div>

                    <div class="response-form">
                        <h2 class="text">Add Response</h2>
                        <form id="resForm">
                            <input type="text" name="subject" class="subject" placeholder="Enter Name" minlength="3" maxlength="20" /><br />
                            <textarea name="question" rows="4" placeholder="Enter Comment" maxlength="1000" ></textarea>
                            <div class="btn-submit"><button id="add-res">Submit</button></div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>

<!-- Footer -->
<footer>
   All Rights Reserved. <i class="fa-regular fa-copyright fa-spin"></i> 2024 Saral ERP Solutions Pvt Ltd. 
    </footer>

<!-- JavaScript to manage active link and dropdown -->
<script>
    const navbarLinks = document.querySelectorAll('.navbar a');
    navbarLinks.forEach(link => {
        link.addEventListener('click', function() {
            navbarLinks.forEach(item => item.classList.remove('active'));
            if (!this.classList.contains('fillApplication-button')) {
                this.classList.add('active');
            }
        });
    });
</script>
<script>
    document.addEventListener('DOMContentLoaded', function() {
        const userIcon = document.getElementById('userIcon');
        const dropdownMenu = document.getElementById('dropdown-menu');
        userIcon.addEventListener('click', function() {
            dropdownMenu.classList.toggle('show');
        });
        window.addEventListener('click', function(event) {
            if (!userIcon.contains(event.target) && !dropdownMenu.contains(event.target)) {
                dropdownMenu.classList.remove('show');
            }
        });
    });
</script>
<%--<script>
var questions = {
	"1" : {
		id: 1,
		title: 'JSON & Web Storage',
		text: 'How to use web Storage',
		votes: 1,
		responses: [
			{
				name: 'Amit',
				comment: 'Local storage can only save strings, so storing objects requires that they be turned into strings using JSON.'
			},
			{
				name: 'Rahul',
				comment: 'Thanks!'
			}
		],
		resolved: false,
		favorite:false
	},
	"2" : {
		id: 2,
		title: 'Web Development',
		text: 'What is Web?',
		votes: 3,
		responses: [
			{
				name: 'Amit',
				comment: 'Web is the Internet.'
			},
			{
				name: 'Rahul',
				comment: 'Thanks!'
			}
		],
		resolved: false,
		favorite:true
	},
	"3" : {
		id: 3,
		title: 'Backend Development',
		text: 'What is Backend?',
		votes: 2,
		responses: [
			{
				name: 'Amir',
				comment: 'Backend is power of a web.'
			},
			{
				name: 'Rahul',
				comment: 'Thanks Amir!'
			}
		],
		resolved: false,
		favorite:false
	}
};

var rightContainer = document.getElementById("right");
var description = document.getElementById("description");
var welcome = document.getElementById("welcome");
var questionItems = document.querySelectorAll(".que-item");
var totalQuestions = 3;

var queTitle = document.getElementById("que-title");
var queText = document.getElementById("que-text");
var responseView = document.getElementById("responses");
var questionView = document.getElementById("questions");
var showWelcomeBtn = document.getElementById("show-welcome");
var addQuestionBtn = document.getElementById("add-que");
var addResponseBtn = document.getElementById("add-res");
var resolveBtn = document.getElementById("resolve");
var search = document.getElementById("search");
var upvote = document.getElementById("upvote");
var downvote = document.getElementById("downvote");
var favorite = document.getElementById("favorite");
var showFavBtn = document.getElementById("show-favorites");

var activeQuestion;

function setupQuestions(){
	questionItems = document.querySelectorAll(".que-item");
	for(let i = 0; i < questionItems.length; i++){
		questionItems[i].addEventListener("click", function(e){
			activeQuestion = questions[this.id];
			console.log(activeQuestion);
			showDescription();
		});
	}
}

function loadQuestions(){

	let data = [];
	questionView.innerText = "";
	for(let queNo in questions){
		let question = questions[queNo];
		data.push(question);
		// questionView.innerHTML += `<div class="que-item" id="${queNo}"><h2 class="que-title">${question.title}</h2><p class="que-text">${question.text}</p></div>`;
	}
	data.sort((a, b)=>{
		return b.votes-a.votes;
	});
	data.forEach((que)=>{
		questionView.innerHTML += `<div class="que-item" id="${que.id}"><h2 class="que-title">${que.title}</h2><p class="que-text">${que.text}</p>${(que.favorite)?"<i class='fa fa-heart active fav'></i>":"<i></i>"}</div>`;
	})
	console.log(data);
	setupQuestions();
}

function addQuestion(event){
	event.preventDefault();
	let form = document.getElementById("formAdd");
	let title = form.subject.value;
	let que = form.question.value;
	if(title.length < 1 || que.length < 1) return false;
	totalQuestions++;
	let id = totalQuestions;
	let newQue = {
		id: id,
		title: title,
		text: que,
		votes: 0,
		responses: [],
		resolved: false
	};
	questions[id] = newQue;
	questionView.innerHTML += `<div class="que-item" id="${newQue.id}"><h2 class="que-title">${newQue.title}</h2><p class="que-text">${newQue.text}</p>${(que.favorite)?"<i class='fa fa-heart active fav'></i>":"<i></i>"}</div>`;
	setupQuestions();
}

function loadActiveQuestion(){
	queTitle.innerText = activeQuestion.title;
	queText.innerText = activeQuestion.text;
	document.getElementById("votes").innerText = activeQuestion.votes;
	loadResponses();
	let form = document.getElementById("resForm");
	form.subject.value = "";
	form.question.value = "";
}

function loadResponses(){
	responseView.innerText = "";
	let responses = activeQuestion.responses;
	responses.forEach((res) => {
		responseView.innerHTML += `<div class="response"><h3 class="res-name">${res.name}</h3><p class="res-text">${res.comment}</p></div>`;
	});
}

function addResponse(event){
	event.preventDefault();
	let form = document.getElementById("resForm");
	let name = form.subject.value;
	let comment = form.question.value;
	if(name.length < 1 || comment.length < 1) return false;
	let newRes = {
		name: name,
		comment: comment
	};
	activeQuestion.responses.push(newRes);
	form.subject.value="";
	form.question.value="";
	loadResponses();
	console.log(questions);
}

function resolveQuestion(){
	delete questions[activeQuestion.id];
	activeQuestion = {};
	loadQuestions();
	showWelcome();
}

function searchQuery(){
	let query = search.value.toLowerCase();
	console.log(query);
	let data = [];
	for(let queNo in questions){
		let question = questions[queNo];
		if(((question['title'].toLowerCase()).indexOf(query) != -1) || ((question['text'].toLowerCase()).indexOf(query) != -1)){
			data.push(question);
		}
	}
	console.log("Data = ", data);

	if(data.length == 0){
		questionView.innerText = "";
		questionView.innerHTML = `<div class="que-item"><h2>No match found!</h2></div>`;
	}else{
		// loading questions 
		questionView.innerText = "";
		data.forEach((que) => {
			let title = que.title;
			let text = que.text;
			title = title.replace(new RegExp(query, 'gi'), '<span class="yellow">' + query + '</span>');
			text = text.replace(new RegExp(query, 'gi'), '<span class="yellow">' + query + '</span>');
			questionView.innerHTML += `<div class="que-item" id="${que.id}"><h2 class="que-title">${title}</h2><p class="que-text">${text}</p>${(que.favorite)?"<i class='fa fa-heart active fav'></i>":"<i></i>"}</div>`;
		});
		setupQuestions();	
	}
}

function showDescription(){
	welcome.style.display="none";
	description.style.display="block";
	loadActiveQuestion();
	setFavorite();
}

function showWelcome(){
	description.style.display="none";
	welcome.style.display="block";
	let form = document.getElementById("formAdd");
	form.subject.value = "";
	form.question.value = "";
}

function upVote(){
	activeQuestion.votes = activeQuestion.votes+1;
	document.getElementById("votes").innerText = activeQuestion.votes;
}

function downVote(){
	activeQuestion.votes = activeQuestion.votes-1;
	document.getElementById("votes").innerText = activeQuestion.votes;
}

function setFavorite(){
	if(activeQuestion.favorite)
		favorite.classList.add("active");
	else
		favorite.classList.remove("active");
}

function addFavorite(){
	activeQuestion.favorite = !activeQuestion.favorite;
	if(activeQuestion.favorite)
		favorite.classList.add("active");
	else
		favorite.classList.remove("active");
	loadQuestions();
}

function showFavorite(){
	if(this.innerText == "View All"){
		loadQuestions();
		this.innerText = "Favorites";
		return;
	}
	//else
	this.innerText = "View All";
	let data = [];
	questionView.innerText = "";
	for(let queNo in questions){
		let question = questions[queNo];
		if(question.favorite)
			data.push(question);
		// questionView.innerHTML += `<div class="que-item" id="${queNo}"><h2 class="que-title">${question.title}</h2><p class="que-text">${question.text}</p></div>`;
	}
	data.sort((a, b)=>{
		return b.votes-a.votes;
	});
	data.forEach((que)=>{
		questionView.innerHTML += `<div class="que-item" id="${que.id}"><h2 class="que-title">${que.title}</h2><p class="que-text">${que.text}</p>${(que.favorite)?"<i class='fa fa-heart active fav'></i>":"<i></i>"}</div>`;		
	})
	console.log(data);
	setupQuestions();
}

function init(){
	loadQuestions();
	showWelcomeBtn.addEventListener("click", showWelcome);
	addQuestionBtn.addEventListener("click", addQuestion);
	addResponseBtn.addEventListener("click", addResponse);
	resolveBtn.addEventListener("click", resolveQuestion);
	search.addEventListener("keyup", searchQuery);
	upvote.addEventListener("click", upVote);
	downvote.addEventListener("click", downVote);
	favorite.addEventListener("click", addFavorite);
	showFavBtn.addEventListener("click", showFavorite);
}

init();
</script>--%>

<script>
function searchQuery(){
	let query = search.value.toLowerCase();
	console.log(query);
	let data = [];
	for(let queNo in questions){
		let question = questions[queNo];
		if(((question['title'].toLowerCase()).indexOf(query) != -1) || ((question['text'].toLowerCase()).indexOf(query) != -1)){
			data.push(question);
		}
	}
	console.log("Data = ", data);

	if(data.length == 0){
		questionView.innerText = "";
		questionView.innerHTML = `<div class="que-item"><h2>No match found!</h2></div>`;
	}else{
		// loading questions 
		questionView.innerText = "";
		data.forEach((que) => {
			let title = que.title;
			let text = que.text;
			title = title.replace(new RegExp(query, 'gi'), '<span class="yellow">' + query + '</span>');
			text = text.replace(new RegExp(query, 'gi'), '<span class="yellow">' + query + '</span>');
			questionView.innerHTML += `<div class="que-item" id="${que.id}"><h2 class="que-title">${title}</h2><p class="que-text">${text}</p>${(que.favorite)?"<i class='fa fa-heart active fav'></i>":"<i></i>"}</div>`;
		});
		setupQuestions();	
	}
}

function init(){
search.addEventListener("keyup", searchQuery);
}
init();
</script>
</body>
</html>
