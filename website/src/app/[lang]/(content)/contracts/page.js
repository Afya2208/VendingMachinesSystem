import {getDictionary} from "@/app/[lang]/dictionaries";
import Link from "next/link";

export default async function ContractsPage({params}){
    var lang = params.lang;
    var t = getDictionary(lang);
    var userId = localStorage.getItem("userId");
    var response = await fetch("")
    var contracts = await response.json();
    // todo
    // получить список договоров для вошедшего пользователя
    return (
        <>
            <h1>{t.contracts.title}</h1>
            <table>
                <thead>
                <tr>
                    <th>{t.contracts.number}</th>
                    <th>{t.contracts.date}</th>
                    <th>{t.contracts.expired}</th>
                    <th>{t.contracts.status}</th>
                    <th>{t.contracts.link}</th>
                </tr>
                </thead>
                <tbody>
                {contracts.map((item, index) => (
                    <tr key={index}>
                        <td>{item.number}</td>
                        <td>{item.date}</td>
                        <td>{item.dateExpired}</td>
                        <td>{item.status}</td>
                        <td>
                            <Link href={`/${lang}/contracts/${item.id}/signing`}>{t.contracts.linkInTable}</Link>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </>
    )
}