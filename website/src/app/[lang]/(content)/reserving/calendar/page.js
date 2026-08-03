import axios from "axios";
import TestInterface from "@/app/[lang]/(content)/test/test-interface";
import https from "https";
export default async function TestPage() {
    var machines = []
    await axios.get("http://localhost:5555/machines", {
        httpsAgent: new https.Agent({rejectUnauthorized: false})
    })
        .then(res=>{
            machines = res.data
        })
    return (
        < >
            <h1>Календарь бронирования</h1>
            <TestInterface machines={machines}/>
        </>
    )
}