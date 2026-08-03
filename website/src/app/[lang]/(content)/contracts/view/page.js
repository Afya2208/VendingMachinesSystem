
import axios from "axios";

import ContractInterface from "@/app/[lang]/(content)/contracts/[contractId]/contract-interface";
import https from "https";

export default async function ContractsPage({params}) {
    var item;
    var contractId = await params.contractId
    await axios.get(`http://localhost:5555/docs/contracts/${contractId}`, {
        httpsAgent: new https.Agent({rejectUnauthorized: false})
    })
        .then(res=>{
            item = res.data;
        });
    //item.document = new Uint8Array(item.document);
    const blob = new Blob([item.document], { type: 'application/pdf' });
    var url = URL.createObjectURL(blob);
    return (
        <>
            <ContractInterface url={url}/>
        </>
    )
}