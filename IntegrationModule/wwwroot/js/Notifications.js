document.addEventListener("DOMContentLoaded", function () {
    fetch("/api/Notification/GetUnsentNotificationsCount")
        .then(response => response.json())
        .then(count => {
            document.querySelector("#unsentNumber").textContent = count;
        });

    fetch("/api/Notification/GetAll")
        .then(response => response.json())
        .then(notifications => {
            const notificationsList = document.querySelector("#notificationList");
            notifications.forEach(notification => {
                const div = document.createElement("div");
                div.classList.add("notification-item");
                div.innerHTML = `
                <div class="notification-subject">${notification.subject}</div>
                <div class="notification-email">${notification.receiverEmail}</div>
                <div class="notification-date">${notification.sentAt}</div>`;
                notificationsList.appendChild(div);
            });
        });


    const sendNotificationsBtn = document.querySelector("#sendBtn");
    sendNotificationsBtn.addEventListener("click", function () {
        fetch("/api/Notification/SendAllNotifications", {
            method: "POST"
        })
            .then(response => {
                if (response.ok) {
                    alert("Notifications sent successfully");
                    fetch("/api/Notification/GetUnsentNotificationsCount")
                        .then(response => response.json())
                        .then(count => {
                            document.querySelector("#unsentNumber").textContent = count;
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
