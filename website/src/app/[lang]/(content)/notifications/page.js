import {getDictionary} from "@/app/dictionaries";
import axios from "axios";
import https from "https";

export default async function NotificationsPage({params}) {
    var lang = params.lang;
    // todo
    // сделать хранение токена и его использование по всему сайту (см. вниз)
    var token = sessionStorage.getItem("token")
    var t = (await getDictionary(lang)).notifications;
    var nots;
    nots = await (await axios.get("https://localhost:7777/notifications", {
        httpsAgent: new https.Agent({rejectUnauthorized: false})
    })).data
    return (
        <>
            <h1>{t.title}</h1>
            <div>
                <table>
                    <thead>
                    <tr>
                        <th>Кто</th>
                        <th>Что сделал</th>
                        <th>Описание</th>
                        <th>Когда</th>
                    </tr>
                    </thead>
                    <tbody>
                    {nots.map(not=>{
                        return(
                            <tr>
                                <td>{not.user.email}</td>
                                <td>{not.what}</td>
                                <td>{not.description}</td>
                                <td>{not.when}</td>
                            </tr>
                        )
                    })}
                    </tbody>
                </table>
            </div>
        </>
    )
}