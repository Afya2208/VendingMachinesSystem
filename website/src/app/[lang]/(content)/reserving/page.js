import {getDictionary} from "@/app/dictionaries";
import ReservingInterface from "@/app/[lang]/(content)/reserving/reserving-interface";

export default async function ReservingPage({params}) {
    var lang = params.lang;
    var t = (await getDictionary(lang)).reserving;
    return (
        <>
            <h1>{t.title}</h1>
            <ReservingInterface t={t}/>
        </>
    )
}