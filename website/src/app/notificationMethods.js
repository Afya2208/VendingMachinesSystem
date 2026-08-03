import axios from "axios";

export default async function PostNotification({description, userId, what}) {
    var token = sessionStorage.getItem("token")
    var date = new Date()
    date.setHours(date.getHours()+3)
    await axios.post("https://localhost:7777/notifications",
        {
            description,
            userId,
            what,
            when: date.toJSON().replace('Z', '')
        },
        {
            headers:{
                Authorization:`Bearer ${token}`
            }
        });
}