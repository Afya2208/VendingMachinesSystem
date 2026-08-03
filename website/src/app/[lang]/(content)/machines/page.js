import {getDictionary} from "@/app/[lang]/dictionaries";
import MachinesInterface from "@/app/[lang]/(content)/machines/machines-interface";

export default async function MachinesPage({params}){
    var lang= params.lang;
    var t = getDictionary(lang);
    var userId = localStorage.getItem("userId");
    var response = await fetch("")
    var machines = await response.json();
    var firstUser = localStorage.getItem("isFirstUser");
    //todo
    // получить через локал сторадже айди пользователя
    return (
        <>
            <h1>{t.main.machines}</h1>
            <MachinesInterface machines={machines} userId={userId} t={t}/>
        </>
    )
}