'use client'


import {useEffect} from "react";

export default  function ContractInterface({url}) {
    useEffect(() => {
        return ()=> URL.revokeObjectURL(url);
    },[])
    return (
        <>
            <h1>Документик</h1>
            <iframe src={url}  width="100%" height="700px" >

            </iframe>
        </>
    )
}