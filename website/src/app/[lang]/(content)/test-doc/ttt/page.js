'use client';

import { useState, useRef, useEffect } from 'react';
import dynamic from 'next/dynamic';
import {  } from 'pdfjs-dist';
import * as pdfjs from "pdfjs-dist";

pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.min.js`;

const PDFViewer = dynamic(
    () => import('@react-pdf-viewer/core').then((mod) => mod.Viewer),
    {
        ssr: false,
        loading: () => <p>Loading PDF viewer...</p>,
    }
);

const SignaturePage = () => {
    const canvasRef = useRef(null);
    const [isDrawing, setIsDrawing] = useState(false);
    const [lastX, setLastX] = useState(0);
    const [lastY, setLastY] = useState(0);
    const [signatureData, setSignatureData] = useState(null);

    const startDrawing = (e) => {
        const canvas = canvasRef.current;
        if (!canvas) return;

        const rect = canvas.getBoundingClientRect();
        const isTouch = e.touches;
        const clientX = isTouch ? e.touches[0].clientX : e.clientX;
        const clientY = isTouch ? e.touches[0].clientY : e.clientY;

        const x = clientX - rect.left;
        const y = clientY - rect.top;

        setIsDrawing(true);
        setLastX(x);
        setLastY(y);
    };

    const draw = (e) => {
        if (!isDrawing) return;
        const canvas = canvasRef.current;
        if (!canvas) return;

        const ctx = canvas.getContext('2d');
        if (!ctx) return;

        const rect = canvas.getBoundingClientRect();
        const isTouch = e.touches;
        const clientX = isTouch ? e.touches[0].clientX : e.clientX;
        const clientY = isTouch ? e.touches[0].clientY : e.clientY;

        const x = clientX - rect.left;
        const y = clientY - rect.top;

        ctx.beginPath();
        ctx.moveTo(lastX, lastY);
        ctx.lineTo(x, y);
        ctx.strokeStyle = '#000';
        ctx.lineWidth = 2;
        ctx.stroke();

        setLastX(x);
        setLastY(y);
    };

    const clearSignature = () => {
        const canvas = canvasRef.current;
        if (!canvas) return;

        const ctx = canvas.getContext('2d');
        if (ctx) {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }
        setSignatureData(null);
    };

    const saveSignature = () => {
        const canvas = canvasRef.current;
        if (!canvas) return;

        const dataUrl = canvas.toDataURL();
        setSignatureData(dataUrl);
        console.log('Signature saved:', dataUrl);
    };

    return (
        <div className="container mx-auto p-4">
            <h1 className="text-2xl font-bold mb-4">Документ для подписи</h1>

            <div className="mb-8" style={{ height: '500px' }}>
                <PDFViewer
                    file="/aa.pdf"
                />
            </div>

            <div className="mb-4">
                <h2 className="text-xl font-semibold mb-2">Область для подписи</h2>
                <canvas
                    ref={canvasRef}
                    width={600}
                    height={200}
                    className="border-2 border-gray-300 rounded"
                    onMouseDown={startDrawing}
                    onMouseMove={draw}
                    onMouseUp={() => setIsDrawing(false)}
                    onMouseLeave={() => setIsDrawing(false)}
                    onTouchStart={startDrawing}
                    onTouchMove={draw}
                    onTouchEnd={() => setIsDrawing(false)}
                />
            </div>

            <div className="flex gap-4">
                <button
                    onClick={clearSignature}
                    className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
                >
                    Очистить
                </button>
                <button
                    onClick={saveSignature}
                    className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                >
                    Сохранить подпись
                </button>
            </div>

            {signatureData && (
                <div className="mt-4">
                    <p className="mb-2">Предпросмотр подписи:</p>
                    <img src={signatureData} alt="Сохраненная подпись" className="border p-2" />
                </div>
            )}
        </div>
    );
};

export default SignaturePage;