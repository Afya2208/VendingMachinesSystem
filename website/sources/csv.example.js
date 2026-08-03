'use client'
import { Parser } from '@json2csv/plainjs';
export default function Home() {
    var data = [
        {value:1, name: 'John Doe'},
        {value:2, name: 'asdasd Doe'},
        {value:3, name: 'John aaDoe'},
    ]
    const click = ()=> {
        var parser = new Parser();
        var csv = parser.parse(data)
        console.log(csv);
    }
    return (
        <>
            <button onClick={click}>sdf</button>
        </>
    );
}
