import {getDictionary} from "@/app/dictionaries";
import RegInterface from "@/app/[lang]/(auth-and-reg)/reg/reg-interface";


export default async function RegPage({params}) {
    var lang = params.lang;
    var t = (await getDictionary(lang)).reg;
    return (
        <>
            <h1>{t.title}</h1>
            <RegInterface t={t}></RegInterface>
        </>
    )
}