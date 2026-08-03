

export default async function HomePage({params}){
    var lang = params.lang;
    const t = await getDictionary(lang);
    return (
        <>
            <h1>{t.main.title}</h1>
            <p>{t.main.text}</p>
            <Link to={`/${lang}/reg`}>{t.main.reg}</Link>
            <Link to={`/${lang}/auth`}>{t.main.auth}</Link>
        </>
    )

}