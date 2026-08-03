'use client';

import { useState, useRef, useEffect } from 'react';

const SignaturePage = () => {
    const canvasRef = useRef(null);
    const [isDrawing, setIsDrawing] = useState(false);
    const [lastPos, setLastPos] = useState({ x: 0, y: 0 });
    const [pdfUrl, setPdfUrl] = useState('/aa.pdf');

    // Инициализация холста
    useEffect(() => {
        const canvas = canvasRef.current;
        if (!canvas) return;

        const ctx = canvas.getContext('2d');
        ctx.lineCap = 'round';
        ctx.lineJoin = 'round';
        ctx.lineWidth = 2;
        ctx.strokeStyle = '#000';
    }, []);

    // Начало рисования
    const startDrawing = (e) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        setIsDrawing(true);
        setLastPos({ x, y });
    };

    // Процесс рисования
    const draw = (e) => {
        if (!isDrawing) return;

        const canvas = canvasRef.current;
        const ctx = canvas.getContext('2d');
        const rect = canvas.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        ctx.beginPath();
        ctx.moveTo(lastPos.x, lastPos.y);
        ctx.lineTo(x, y);
        ctx.stroke();

        setLastPos({ x, y });
    };

    // Очистка подписи
    const clearSignature = () => {
        const canvas = canvasRef.current;
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    };

    return (
        <div className="container mx-auto p-4">
            <h1 className="text-2xl font-bold mb-4">Подписание документа</h1>

            {/* PDF Viewer */}
            <div className="mb-8 h-[500px]">
                <iframe
                    src={pdfUrl}
                    className="w-full h-full border rounded"
                    title="PDF Viewer"
                />
            </div>

            {/* Область для подписи */}
            <div className="mb-6">
                <h2 className="text-lg font-semibold mb-2">Нарисуйте подпись:</h2>
                <canvas
                    ref={canvasRef}
                    width={600}
                    height={200}
                    className="border-2 border-gray-300 rounded touch-none"
                    onMouseDown={startDrawing}
                    onMouseMove={draw}
                    onMouseUp={() => setIsDrawing(false)}
                    onMouseLeave={() => setIsDrawing(false)}
                />
            </div>

            {/* Кнопки управления */}
            <div className="flex gap-3">
                <button
                    onClick={clearSignature}
                    className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
                >
                    Очистить
                </button>
                <button
                    onClick={() => {
                        const dataUrl = canvasRef.current.toDataURL();
                        console.log('Подпись сохранена:', dataUrl);
                        alert('Подпись сохранена! Проверьте консоль.');
                    }}
                    className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                >
                    Сохранить
                </button>
            </div>
        </div>
    );
};

export default SignaturePage;