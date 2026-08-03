export default async function NotFound({params}){
    var lang = params.lang;
    const t = await getDictionary(lang);
    return (
        <>
            <h1>{t.main.title}</h1>
            <p>{t.main.text}</p>

        </>
    )

}