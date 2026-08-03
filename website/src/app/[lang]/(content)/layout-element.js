'use client'

import styles from "@/app/styles/contentLayout.module.css";
import Link from "next/link";
import PostNotification from "@/app/notifications";

export default function LayoutElement({lang, t}) {
    var userId = localStorage.getItem("userId")
    return (
        <div className={styles.sidebar}>
            <p><ClickLink href={`/${lang}/main`} name={t.main.title} userId={userId}/></p>
            <p><Link href={`/${lang}/main`}>{t.main.title}</Link></p>
            <p><Link href={`/${lang}/settings`}>{t.settings.title}</Link></p>
            <p><Link href={`/${lang}/reserving`}>{t.reserving.title}</Link></p>
            <p><Link href={`/${lang}/machines`}>{t.machines.title}</Link></p>
            <p><Link href={`/${lang}/contracts`}>{t.contracts.title}</Link></p>
            <p><Link href={`/${lang}/notifications`}>{t.notifications.title}</Link></p>
        </div>
    );
}


function ClickLink({href, name, userId}) {
    const click = async ()=>{
        await PostNotification({
            description:"",
            userId,
            what:`Переход на страницу ${name}`
        })
    }
    return(
        <Link onClick={click} href={href}>{name}</Link>
    )
}