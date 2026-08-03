'use client'

import {useEffect, useState} from "react";
import axios from "axios";
import PostNotification from "@/app/notifications";

var machines = [];
export default  function ReservingInterface({t}) {
    const [filteredMachines, setFilteredMachines] = useState([]);
    const [machinesIds, setMachinesIds] = useState([]);
    const [models, setModels] = useState([]);
    var userId =  localStorage.getItem("userId");
    const fetchData = async () => {

        await axios.get(`https://localhost:7777/machines`, {
            headers: {
                Authorization:`Bearer ${sessionStorage.getItem("token")}`
            }
        })
            .then(res=>{
                machines = res.data;

            })
        await axios.get(`https://localhost:7777/machines/models`, {
            headers: {
                Authorization:`Bearer ${sessionStorage.getItem("token")}`
            }
        })
            .then(res=>{
                setModels(res.data);

            })
        filterAndSort()
    }
    const [reservingOptions, setReservingOptions] = useState({
        dateEnd:"",
        dateStart:"",
        insurance:false,
        way:""
    });
    const makeReserving = async () => {
        if (confirm("Вы подтверждаете это бронирование?")) {
            var list = []
            var item = {}
            machinesIds.forEach((id) => {list.push(
                {
                    machineId: id,
                    userId: userId,
                    insurance: reservingOptions.insurance === "true",
                    way: reservingOptions.way,
                    dateEnd: reservingOptions.dateEnd,
                    dateStart: reservingOptions.dateStart,
                }
            )})
            await axios.post(`https://localhost:7777/docs/reserving`, list)
                .then(res=>{
                    PostNotification({
                        description:"", what:"Успешное бронирование", userId:userId
                    })
                })
                .catch(err=>{
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
    const filterAndSort = () => {
        if (filterOptions.modelId!=0)
            machines = machines.filter(machine=>machine.modelId == filterOptions.modelId)
        if (filterOptions.statusId!="")
            machines = machines.filter(machine=>machine.reservingStatus == filterOptions.statusId)
        if (sortOptions == "2")
            machines.sort((a,b) => { return  a.timeToGetSuccess - b.timeToGetSuccess})
        if (sortOptions == "1")
            machines.sort((a,b) => {return  b.timeToGetSuccess - a.timeToGetSuccess})
        setFilteredMachines(machines)
    }
    const [filterOptions, setFilterOptions] = useState({
        modelId:0,
        statusId:""
    });
    const [sortOptions, setSortOptions] = useState(1)
    useEffect(() => {
        fetchData();
    }, [filterOptions.statusId, filterOptions.modelId, sortOptions]);
    return (
        <>
            <div>
                <p>Фильтры</p>
                <p>Статус</p>
                <p>
                    <select value={filterOptions.statusId} onChange={(e)=>setFilterOptions({...filterOptions, statusId: e.target.value})}>
                        <option value={"Забронирован"}>Забронирован</option>
                        <option value={"Доступен"}>Доступен</option>
                    </select>
                </p>
                <p>Модель</p>
                <p>
                    <select value={filterOptions.modelId} onChange={(e)=>setFilterOptions({...filterOptions, modelId: e.target.value})}>
                        {models.map(model=>{
                            return( <option value={model.id}>{model.name}</option>)
                        })}
                    </select>
                </p>
                <p>Сортировка</p>
                <p>
                    <select value={sortOptions} onChange={(e)=>setSortOptions(e.target.value)}>
                        <option value={1}>По убыванию срока окупаемости</option>
                        <option value={2}>По возрастанию срока окупаемости</option>
                    </select>
                </p>
                <p>
                    <select size={10} multiple value={machinesIds} onChange={(e)=>{
                        var items = Array.from(e.currentTarget.selectedOptions)
                        var ids = []
                        items.forEach(x=>ids.push(x.value))
                        setMachinesIds(ids)
                    }}>
                        {filteredMachines.map(machine=>{
                            return (
                                <option value={machine.id}>{machine.name}, модель - {machine.model?.name} {machine.rentMonthly} р. за месяц {machine.rentYearly} р. за год, статус: {machine.reservingStatus}, находится в {machine.address}, срок окупаемости (в месяцах): {machine.timeToGetSuccess}</option>
                            )
                        })}
                    </select>
                </p>
                <p>
                    <button onClick={()=>{
                        var common = document.getElementById("common");
                        common.hidden = false;
                    }}>Перейти к бронированию</button>
                    <button>Перейти к календарю бронирования</button>
                </p>
                <div hidden id={"common"}>
                    <p>Дата начала бронирования <input type={"date"} value={reservingOptions.dateStart} onChange={(e)=>setReservingOptions({...reservingOptions, dateStart: e.target.value})}/></p>
                    <p>Дата окончания бронирования <input type={"date"} value={reservingOptions.dateEnd} onChange={(e)=>setReservingOptions({...reservingOptions, dateEnd: e.target.value})}/></p>
                    <p>Страхование (да/нет)<input type={"checkbox"} value={reservingOptions.insurance}  onChange={(e)=>setReservingOptions({...reservingOptions, insurance: e.target.value})}/></p>
                    <p><select value={reservingOptions.way} onChange={(e)=>setReservingOptions({...reservingOptions, way: e.target.value})}>
                        <option value={"Покупка"}>Покупка</option>
                        <option value={"Аренда"}>Аренда</option>
                    </select></p>
                    <p><button onClick={makeReserving}>Забронировать</button></p>
                </div>
            </div>
        </>
    )
}