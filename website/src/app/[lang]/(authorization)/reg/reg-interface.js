'use client'
import {use, useState} from "react";
import axios from "axios";
import {useRouter} from "next/navigation";

var captchaResult = "no"
var emailCode = "no"

export default function RegInterface({t}) {
    const [userInfo, setUserInfo] = useState({
        email:"",
        password:"",
        code:""
    })
    const [errors, setErrors] = useState({
        email:true,
        password:true,
        code:true,
        emailCode:true,
        captcha:true,
    })
    const [userWrote, setUserWrote] = useState({
        captcha:"",
        emailCode:""
    })
    const [captcha, setCaptcha] = useState("")
    const createCaptcha = ()=>{
        var f = Math.floor(Math.random()*20)
        var s = Math.floor(Math.random()*20)
        var t = Math.floor(Math.random()*20)
        var fo = Math.floor(Math.random()*20)
        setCaptcha(`${f} + ${s} - ${t} * ${fo} = `)
        captchaResult = f + s - t * fo;
        setUserWrote({...userWrote, captcha: ""})
        setErrors({...errors, captcha: true})
    }
    const checkCaptcha = () =>{
        if (userWrote.captcha == captchaResult) {
            setErrors({...errors, captcha: false})
        }
    }
    const checkCode = async ()=>{
        await axios.post("", userInfo.code)
            .then(res=>{
                if (res.data == true) {
                    setErrors({...errors, code: false})
                }
            })
    }
    const checkPassword = (e) => {
        setUserInfo({...userInfo, password: e.target.value})
        var text = e.target.value
        if (text.length < 8 ||
            !/[0-9]/.test(text)
            || !/[a-zA-Z]/.test(text) ||
            !/[^a-zA-Z0-9]/.test(text)) {
            setErrors({...errors, password: true})
        }
        else {
            setErrors({...errors, password: false})
        }
    }
    const router = useRouter()
    const createEmailCode = () =>{
        emailCode = Math.floor(Math.random()*6666)
        alert(emailCode)
    }
    const checkEmail = (e) => {
        setUserInfo({...userInfo, email: e.target.value})
        var text = e.target.value
        if (!/./.test(text)
            || !/@/.test(text)) {
            setErrors({...errors, email: true})
        }
        else {
            setErrors({...errors, email: false})
        }
    }
    const reg = async () =>{
        await axios.post("https://localhost:7777/users/reg", userInfo)
            .then(res=>{

            })
    }
    return (
        <>
            <div>
                {t.email}
                <input value={userInfo.email} onChange={(e)=>checkEmail(e)}/>
            </div>
            <div>

                <input value={userInfo.email} onChange={(e)=>checkEmail(e)}/>
            </div>
            <div>
                {t.password}
                <input value={userInfo.password} onChange={(e)=>checkPassword(e)}/>
            </div>

            <p hidden={!errors.email}>
                {t.errors.email}
            </p>
            <p hidden={!errors.password}>
                {t.errors.password}
            </p>
            <p hidden={!errors.emailCode}>
                {t.errors.emailCode}
            </p>
            <p hidden={!errors.code}>
                {t.errors.code}
            </p>
            <p hidden={!errors.captcha}>
                {t.errors.captcha}
            </p>
            <p>
                <button onClick={reg} disabled={errors.code||errors.emailCode||errors.email||errors.password||errors.captcha}>{t.button}</button>
            </p>
        </>
    )
}