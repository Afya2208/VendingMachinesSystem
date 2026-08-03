'use client'



import {Calendar} from "antd";
import {Calendar as ReactCalendar} from "react-calendar";
import {useState} from "react";
import styles from '@/app/styles/calendar.module.css'

export default function ReservingCalendar() {
    const [selectedDates, setSelectedDates] = useState({
        dateStart:"",
        dateEnd:"",
    })
    var freeDays = []
    const defineColor = ({date, view})=>{
        if (view === "month") {
            if (freeDays.find(x=>x === date)) {
                return styles.freeDay
            }
            else {
                return styles.takenDay
            }
        }
    }
    return (
        <>
            <p>Календарь из Antd</p>
            <Calendar></Calendar>
            <p>Календарь из react-calendar</p>
            <ReactCalendar
                onChange={(e)=>setSelectedDates({...selectedDates, dateStart: e})}
                value={selectedDates.dateStart}
                tileClassName={defineColor}
            ></ReactCalendar>
            <button onClick={()=>{
                alert(selectedDates.dateStart.toJSON())
            }}>Check select</button>
        </>
    )
}