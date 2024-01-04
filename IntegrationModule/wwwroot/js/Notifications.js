﻿document.addEventListener("DOMContentLoaded", function () {
    fetch("/api/Notifications/GetUnsentNotificationsCount")
        .then(response => response.json())
        .then(count => {
            document.querySelector("#unsentNotifNumber").textContent = count;
        });

    fetch("/api/Notifications/GetAll")
        .then(response => response.json())
        .then(notifications => {
            const notificationsList = document.querySelector("#notificationList");
            notifications.forEach(notification => {
                const div = document.createElement("div");
                div.id = "notification";
                div.innerHTML = `
            <div>${notification.subject}</div>
            <div>${notification.receiverEmail}</div>
            <div>${notification.sentAt}</div>`;
                notificationsList.appendChild(div);
            });
        });

    const sendNotificationsBtn = document.querySelector("#sendBtn");
    sendNotificationsBtn.addEventListener("click", function () {
        fetch("/api/Notifications/SendAllNotifications", {
            method: "POST"
        })
            .then(response => {
                if (response.ok) {
                    alert("Notifications sent successfully");
                    fetch("/api/Notifications/GetUnsentNotificationsCount")
                        .then(response => response.json())
                        .then(count => {
                            document.querySelector("#unsentNotifNumber").textContent = count;
                        });
                } else {
                    alert("Error sending notifications");
                }
            })
            .catch(error => {
                alert("Error sending notifications");
            });
    });
});