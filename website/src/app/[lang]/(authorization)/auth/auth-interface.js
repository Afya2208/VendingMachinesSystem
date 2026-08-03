'use client'
import {use, useState} from "react";
import axios from "axios";
import {useRouter} from "next/navigation";
import Link from "next/link";
import PostNotification from "@/app/notifications";

export default function AuthInterface({t, lang}) {
    const [userInfo, setUserInfo] = useState({
        email:"",
        password:"",
        code:""
    })
    const [errors, setErrors] = useState({
        email:true,
        password:true,
        code:true,
    })
    const checkCode = async ()=>{
        await axios.post(`https://localhost:5555/users/checkCode`, {code :userInfo.code, email:userInfo.email})
            .then(res=>{
                if (res.data == true) {
                    setErrors({...errors, code: false})
                }
                else {
                    setErrors({...errors, code: true})
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
    const checkEmail = (e) => {
        setUserInfo({...userInfo, email: e.target.value})
        var text = e.target.value
        if (!/[.]/.test(text)
            || !/@/.test(text)) {
            setErrors({...errors, email: true})
        }
        else {
            setErrors({...errors, email: false})
        }
    }
    const auth = async () =>{
        await axios.post("https://localhost:5555/users/auth", {
            email:userInfo.email,
            password:userInfo.password
        })
            // обязательно только такая форма res=>{действия}
            // нельзя res=>()=>{...}
            .then(res=>{
                localStorage.setItem("userId", res.data.user.id)
                sessionStorage.setItem("token", res.data.code)
                localStorage.setItem("isFirst", res.data.user.isFirstOnWebsite)
                alert("Успешная авторизация")
                router.push(`/${lang}/main`)
                PostNotification({
                    description:"",
                    userId:res.data.user.id,
                    what:"Успешная авторизация"
                })
            })
            .catch(err=>{
                alert("Неуспешная авторизация: неправильный пароль или логин")
                alert(err)
            })
    }
    return (
        <>
            <div>
                {t.email}
                <input value={userInfo.email} onChange={(e)=>checkEmail(e)}/>
            </div>
            <div>
                {t.password}
                <input value={userInfo.password} onChange={(e)=>checkPassword(e)}/>
            </div>
            <div>
                {t.code}
                <input value={userInfo.code} onChange={(e)=>setUserInfo({...userInfo, code: e.target.value})}/>
                <button onClick={checkCode}>{t.checkCodeButton}</button>
            </div>
            <p hidden={!errors.email}>
                {t.errors.email}
            </p>
            <p hidden={!errors.password}>
                {t.errors.password}
            </p>
            <p hidden={!errors.code}>
                {t.errors.code}
            </p>
            <p>
                <button onClick={auth} disabled={errors.code||errors.email||errors.password}>{t.button}</button>
            </p>
            <Link href={`${lang}/reg`}>{t.reg}</Link>
        </>
    )
}