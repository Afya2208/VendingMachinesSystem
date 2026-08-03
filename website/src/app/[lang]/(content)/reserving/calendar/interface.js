'use client'

import {useEffect, useState} from "react";
import axios from "axios";
import {Calendar} from "react-calendar";
import "react-calendar/dist/Calendar.css";
import styles from "@/app/styles/calendar.module.css";
import https from "https";
import PostNotification from "@/app/notifications";

var count = 0;
var takenDays = []
var unconfirmedDays = []
export default function TestInterface({machines}) {
    const [selectedMachineId, setSelectedMachineId] = useState("0")
    const [reservingOptions, setReservingOptions] = useState({
        way:"",
        insurance:false,
        dateEnd:undefined,
        dateStart:undefined,
    })
    const [machineReservingDates, setMachineReservingDates] = useState({
        takenDays:[],
        unconfirmedDays:[]
    })
    const fetchData = async ()=>{
        if (selectedMachineId !== "0")
            await axios.get(`https://localhost:7777/machines/${selectedMachineId}`,{
                httpsAgent: new https.Agent({rejectUnauthorized: false})
            })
                // интересно то, что предыдущие ответы сохраняются, поэтому важно, чтобы запрос выполнялся нормально
                .then(res=>{
                    var items = res.data
                    takenDays = items.takenDays.map(item=>new Date(item));
                    unconfirmedDays = items.unconfirmedDays.map(item=>new Date(item));
                    console.log(takenDays)
                    console.log(unconfirmedDays)
                    setMachineReservingDates({...machineReservingDates, takenDays: takenDays})
                    setMachineReservingDates({...machineReservingDates, unconfirmedDays: unconfirmedDays})
                })
    }
    const defineDateCells = ({date, view}) =>{

        if (view === "month") {
            date.setHours(3,0,0,0)

            if (takenDays.find(x=>x.getTime() === date.getTime())) {
                return styles.blockedCell
            }
            if (unconfirmedDays.find(x=>x.getTime() === date.getTime())) {
                return styles.unconfirmedCell
            }
            if (date.getTime() === reservingOptions.dateStart?.getTime() || date.getTime() === reservingOptions.dateEnd?.getTime()) {
                return styles.selectedCell
            }
            else {
                return styles.freeCell
            }
        }
    }
    const reserve =async () =>{
        if (confirm("Вы подтверждаете это бронирование?")) {
            var list = []
            list.push({
                machineId:parseInt(selectedMachineId),
                userId:1,
                dateStart:reservingOptions.dateStart.toJSON().substring(0, 10),
                dateEnd:reservingOptions.dateEnd.toJSON().substring(0, 10),
                way:reservingOptions.way,
                insurance:reservingOptions.insurance == "true"
            })
            console.log(list[0])
            await axios.post(`https://localhost:7777/docs/reserving`, list)
                .then(res=>{
                    alert(res)
                    PostNotification({
                        description:"", what:"Успешное бронирование", userId:userId
                    })
                })
                .catch(err=>{
                    alert(err)
                    alert(err)
                    PostNotification({
                        description:"Ошибка сети", what:"Неуспешная попытка бронирования", userId:userId
                    })
                })
        }
        else {
            alert("Бронирование не удалось");
            await PostNotification({
                description:"Заявка отклонена", what:"Неуспешная попытка бронирования", userId:userId
            })
        }
    }
    const chooseDate = (e) => {
        count++;
        e.setHours(3,0,0,0)
        if (count == 1) {
            setReservingOptions({...reservingOptions, dateStart: e})
        }
        else if (count == 2){
            count = 0;
            setReservingOptions({...reservingOptions, dateEnd: e})
        }
    }
    useEffect(e=>{
        fetchData()
    }, [selectedMachineId])
    return (
        <div className={styles.specDiv}>
            <p>
                <select size={1} value={selectedMachineId} onChange={(e)=>setSelectedMachineId(e.target.value)}>
                    <option value={0}>Не выбран</option>
                    {machines.map(machine=>{
                        return (
                            <option value={machine.id}>{machine.id} {machine.name} {machine.address}</option>
                        )
                    })}
                </select>
            </p>

            <Calendar minDate={new Date()} onChange={(e)=>chooseDate(e)} tileClassName={defineDateCells}>

            </Calendar>
            <p>Выбранная дата начала: {(()=>{
                if (reservingOptions.dateStart !== undefined) return reservingOptions.dateStart.toLocaleDateString()
                else return "";
            })()}</p>
            <p>Выбранная дата окончания: {(()=>{
                if (reservingOptions.dateEnd !== undefined) return reservingOptions.dateEnd.toLocaleDateString()
                else return "";
            })()}</p>
            <p>Способ ведения <select value={reservingOptions.way} onChange={(e)=>setReservingOptions({...reservingOptions, way: e.target.value})}>
                <option value={"Продажа"}>Продажа</option>
                <option value={"Аренда"}>Аренда</option>
            </select></p>
            <p>Страхование (да/нет)<input value={reservingOptions.insurance} type={"checkbox"} onChange={(e)=>setReservingOptions({...reservingOptions, insurance: e.target.value})}/></p>
            <p><button onClick={reserve}>Забронировать</button></p>
        </div>
    )
}