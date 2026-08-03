import {getDictionary} from "@/app/[lang]/dictionaries";

export default async function MainPage({params}){
    var lang= params.lang;
    var t = getDictionary(lang).main;
    return (
        <>
            <h1>{t.title}</h1>
        </>
    )
}