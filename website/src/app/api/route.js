import {NextResponse} from "next/server";
import * as path from "node:path";
import * as fs from "node:fs";

// запрос /api/gen-contract
export async function GET(req) {
    const {searchParams} = new URL(req.url)
    const userId = searchParams.get("userId")

    try {
        var response = await fetch("")
        // если ответ это чисто файл
        var buffer = await response.arrayBuffer()
        // путь к папке contracts
        var directory = path.join(process.cwd(), 'contracts')
        const fileNameEx = `contract_${userId}_${Date.now()}.pdf`;
        var fileName = ""
        var fileFullName = path.join(directory, fileName)
        // создание папки (если вдруг ее нет)
        await fs.mkdir(directory, {recursive:true})
        // создание файла
        await fs.writeFile(fileFullName, Buffer.from(buffer))
        // создание ссылки на файл
        var ulr = "/contracts"
        return NextResponse.json({
            buffer
        })
        // return NextResponse.json({ fileUrl: `/contracts/${fileName}` });

    } catch (e) {
        return NextResponse.json(
            {error: 'Internal Server Error'},
            {status: 500}
        )
    }
}