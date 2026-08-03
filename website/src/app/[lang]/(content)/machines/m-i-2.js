'use client'

import {createElement, useEffect, useState} from "react";
import axios from "axios";
import {Pagination} from "antd";
import {Parser} from "@json2csv/plainjs";
import styles from '@/app/styles/machines.module.css'


export default function MachinesInterface({t}) {
    const [totalCount, setTotalCount] = useState(0);
    const [valuesBlock, setValuesBlock] = useState("");
    const [machines, setMachines] = useState([]);
    const [displayTable, setDisplayTable] = useState(true)
    const [pageSettings, setPageSettings] = useState({
        pageLimit: 10,
        pageNumber: 1,
        searchText: ""
    })
    const fetchData = async () => {
        var userId = await localStorage.getItem("userId");
        await axios.get(`https://localhost:7777/machines/${userId}/pages?pageLimit=${pageSettings.pageLimit}&pageNumber=${pageSettings.pageNumber}&searchText=${pageSettings.searchText}`, {
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem("token")}`
            }
        })
            .then(res => {
                setMachines(res.data.vendingMachines);
                setTotalCount(res.data.totalCount);
                var value = (pageSettings.pageNumber - 1) * pageSettings.pageLimit
                setValuesBlock(`Записи с ${(res.data.vendingMachines.length == 0 ? 0 : value + 1)} по ${value + res.data.vendingMachines.length}`)
            })
        var isFirst = localStorage.getItem("isFirst")
        // тут почему-то важно сделать именно такое сравнение
        if (isFirst === "true") {
            alert("Оформите ТА в аренду")
            localStorage.setItem("isFirst", false)
            await axios.put(`https://localhost:7777/users/${userId}/no-first`, "")
        }
    }

    const exporT = () => {
        const parser = new Parser()
        var text = parser.parse(machines)
        console.log(text)
        /*
        var linkA = createElement('a')
        linkA.download="файлик.csv"
        linkA.href = url;
        linkA.click();
        URL.revokeObjectURL(url)

         */
    }
    useEffect(() => {
        fetchData();
        var iterval = setInterval(fetchData, 10000);
        return () => clearInterval(iterval)
    }, []);
    useEffect(() => {
        fetchData();
    }, [pageSettings.pageLimit, pageSettings.pageNumber, pageSettings.searchText]);
    return (
        <>
            <div>
                <span style={{float: "left"}}>
                    Торговые автоматы
                </span>
                <span style={{float: "right"}}>
                    <button onClick={() => {
                        setDisplayTable(true)
                    }}>Таблица</button>
                    <button onClick={() => {
                        setDisplayTable(false)
                    }}>Плитка</button>
                </span>
            </div>
            <div style={{clear: "both"}}></div>
            <p>Всего найдено {totalCount} шт.</p>
            <div>
                <span style={{float: "left"}}>
                    Показать
                    <select value={pageSettings.pageLimit}
                            onChange={(e) => setPageSettings({...pageSettings, pageLimit: e.target.value})}>
                        <option value={10}>10</option>
                        <option value={20}>20</option>
                    </select>
                    записей
                </span>
                <span style={{float: "left"}}>
                    <input placeholder={"Фильтр"} value={pageSettings.searchText}
                           onChange={(e) => setPageSettings({...pageSettings, searchText: e.target.value})}/>
                </span>
                <span style={{float: "right"}}>
                    <button>Добавить</button>
                    <button onClick={exporT}>Экспорт</button>
                </span>
            </div>
            <div style={{clear: "both"}}></div>

            <div hidden={!displayTable}>
                <table className={styles.tableItems} style={{border: "solid 2px black"}}>
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>Название автомата</th>
                        <th>Модель</th>
                        <th>Компания</th>
                        <th>Модем</th>
                        <th>Адрес/место</th>
                        <th>В работе с</th>
                        <th>Действия</th>
                    </tr>
                    </thead>
                    <tbody>
                    {machines.map(m => {
                        return (
                            <>
                                <tr>
                                    <td>{m.id}</td>
                                    <td>{m.name}</td>
                                    <td>{m.model?.name}</td>
                                    <td>{m.renterCompany?.name}</td>
                                    <td>{m.modem}</td>
                                    <td>{m.address} {m.place}</td>
                                    <td>{m.dateInstalled}</td>
                                    <td><span>
                                <button>✏</button>
                                <button>🗑</button>
                                <button>🔓</button>
                            </span></td>
                                </tr>
                            </>
                        )
                    })}
                    </tbody>
                </table>
            </div>
            <div hidden={displayTable} className={styles.list}>
                <ul>
                    {machines.map(m => {
                        return (
                            <>
                                <li>
                                    <div className={styles.item}>
                                        <p>{m.id}</p>
                                        <p>{m.name}</p>
                                        <p>{m.model?.name}</p>
                                        <p>{m.renterCompany?.name}</p>
                                        <p>{m.modem}</p>
                                        <p>{m.address} {m.place}</p>
                                        <p>{m.dateInstalled}</p>
                                        <p>
                                    <span>
                                <button>✏</button>
                                <button>🗑</button>
                                <button>🔓</button>
                                 </span>
                                        </p>
                                    </div>
                                </li>
                            </>
                        )

                    })}
                </ul>
            </div>
            <p>{valuesBlock}</p>
            <Pagination total={totalCount} pageSize={pageSettings.pageLimit}
                        onChange={(e) => setPageSettings({...pageSettings, pageNumber: e})}></Pagination>
        </>
    )
}