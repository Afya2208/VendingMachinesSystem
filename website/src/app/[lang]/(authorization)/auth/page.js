import {getDictionary} from "@/app/dictionaries";
import AuthInterface from "@/app/[lang]/(auth-and-reg)/auth/auth-interface";


export default async function AuthPage({params}) {
    var lang = params.lang;
    var t = (await getDictionary(lang)).auth;
    return (
        <>
            <h1>{t.title}</h1>
            <AuthInterface t={t} lang={lang}/>
        </>
    )
}