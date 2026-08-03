import {getDictionary} from "@/app/dictionaries";
import styles from '@/app/styles/contentLayout.module.css'
import Link from "next/link";
import PostNotification from "@/app/notifications";
import LayoutElement from "@/app/[lang]/(content)/layout-element";



export default async function ContentLayout({ children, params }) {
    var lang = await params.lang;
    var t = await getDictionary(lang);
    return (
        <div>
            <LayoutElement lang={lang} t={t}/>
            <div className={styles.content}>
                {children}
            </div>
        </div>
    );
}
