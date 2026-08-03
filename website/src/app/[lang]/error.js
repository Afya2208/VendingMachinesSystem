export default async function Error({params}){
    var lang = params.lang;
    const t = await getDictionary(lang);
    return (
        <>
            <em>{t.error.title}</em>
        </>
    )

}