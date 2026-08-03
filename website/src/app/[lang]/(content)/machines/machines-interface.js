'use client'


import {useEffect, useState} from "react";

export default async function MachinesInterface({t, machines, total, userId}){
    const [pageLimit, setPageLimit] = useState(10);
    const [currentPage, setCurrentPage] = useState(1);
    useEffect(() => {
        const fetchData = async () => {
            var response = await fetch(`/`)
            machines = await response.json();
        }
        fetchData();
    },[pageLimit, currentPage]);
    return (
        <>
            <p>{t.machines.title}</p>
            <p>{t.machines.total} {total}</p>
            <p>
                <span>{t.machines.show1}</span>
                <select value={pageLimit} onChange={(e)=>setPageLimit(e.target.value)}>
                    <option>10</option>
                    <option>20</option>
                </select>
                <span>{t.machines.show2}</span>
            </p>
            <table>
                <thead>
                <tr>
                    <th>ID</th>
                    <th>{t.machines.name}</th>
                    <th>{t.machines.model}</th>
                    <th>{t.machines.company}</th>
                    <th>{t.machines.modem}</th>
                    <th>{t.machines.addressPlace}</th>
                    <th>{t.machines.dateStart}</th>
                    <th>{t.machines.actions}</th>
                </tr>
                </thead>
                <tbody>
                {machines.map((machine) => {
                    <tr>
                        <td>{machine.id}</td>
                        <td>{machine.name}</td>
                        <td>{machine.model.name}</td>
                        <td>{machine.company.name}</td>
                        <td>{machine.modem.id}</td>
                        <td>{machine.addres}/{machine.place}</td>
                        <td>
                            <button>✏️</button>
                            <button>🗑️</button>
                            <button>🔓</button>
                        </td>
                    </tr>
                })}
                </tbody>
            </table>
        </>
    )
}