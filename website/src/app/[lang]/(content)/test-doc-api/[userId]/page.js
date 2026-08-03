'use client';

import { useEffect, useState } from 'react';

export default function ContractPage({ params }) {
    const [pdfUrl, setPdfUrl] = useState(null);
    const [error, setError] = useState('');

    useEffect(() => {
        const generateContract = async () => {
            try {
                const response = await fetch(`/api?userId=${params.userId}`);
                const { fileUrl } = await response.json();
                setPdfUrl(`${fileUrl}?t=${Date.now()}`);
            } catch (err) {
                setError('Ошибка при генерации документа');
            }
        };

        generateContract();
    }, [params.userId]);

    if (error) return <div>{error}</div>;
    if (!pdfUrl) return <div>Генерация документа...</div>;

    return (
        <iframe
            src={pdfUrl}
            width="100%"
            height="600px"
            title="Договор"
        />
    );
}