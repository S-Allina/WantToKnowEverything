"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var IdGroup = document.getElementById("IdGroup").value;
var meName = document.getElementById("me").value;

const chatHub = connection.start().then(() => connection.invoke('LoadHistory', IdGroup));
connection.on('LoadHistory', (messages) => {
    console.log(meName)
    console.log(messages)
    const messagesList = document.getElementById('messagesList');
    messages.forEach(message => {
        const li = document.createElement("li");
        li.classList.add("message-box");
        /*li.classList.add(message.role !== "user" ? "messageFromTeacher" : "");*/
        if (meName !== message.senderUserName) {
            if (message.role !== "user") {
                li.classList.add("left", "messageFromTeacher");
            } else {
                li.classList.add("left");
            }
        } else {
           
                li.classList.add("right");
            
        }
        //li.classList.add(meName !== message.senderUserName ? "left" : "right");
     
        li.textContent = `${message.senderName}: ${message.text} `;
        messagesList.appendChild(li);
    });
   preventDefault();
});

var you = document.querySelector("#You").textContent.split(' ');





connection.on('x', (UserName,FirstName, message) => {
    console.log("99");
    console.log(message);
    var li = document.createElement("li");
    li.classList.add("message-box");
    li.classList.add(meName !== UserName ? "left" : "right");
    //li.classList.add(role !== "user" ? "messageFromTeacher" : "");
    document.getElementById("messageInput").value = "";
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${FirstName}: ${message}`;
});


connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var group = document.getElementById("IdGroup").value;
    var message = document.getElementById("messageInput").value;
    if (message == "") {
        alert("Нельзя отправить пустое сообщение.");
        return;
    }
    document.getElementById("messageInput").value = "";
    connection.invoke("SendMessage", group, meName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
})




