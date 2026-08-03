import {getDictionary} from "@/app/dictionaries";

export default async function SettingsPage({params}) {
    var lang = params.lang;
    var t = (await getDictionary(lang)).settings;
    return (
        <>
            <h1>{t.title}</h1>
        </>
    )
}