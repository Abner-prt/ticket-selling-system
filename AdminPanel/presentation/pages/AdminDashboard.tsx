import { useEffect, useState } from 'react';
import { getEventsAction, deleteEventAction } from '../../core/actions/admin.actions';
import { EventDto } from '../../infrastructure/interfaces/DTOs';

export const AdminDashboard = () => {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [loading, setLoading] = useState(true);

    const fetchEvents = async () => {
        setLoading(true);
        try {
            const res = await getEventsAction(1, '');
            setEvents(res.data.items || []);
        } catch (error) {
            console.error("Error fetching events", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEvents();
    }, []);

    const handleDelete = async (id: number) => {
        if(confirm("¿Seguro que deseas eliminar este evento?")) {
            await deleteEventAction(id);
            fetchEvents();
        }
    };

    return (
        <div className="min-h-screen bg-slate-900 text-slate-200 p-8">
            <div className="max-w-6xl mx-auto">
                
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-3xl font-bold text-white">
                        Panel de <span className="text-orange-500">Administración</span>
                    </h1>
                    <button className="bg-orange-500 hover:bg-orange-600 text-white px-4 py-2 rounded font-bold">
                        Nuevo Evento
                    </button>
                </div>

                <div className="bg-slate-800 rounded-lg shadow-lg border border-slate-700">
                    <div className="p-4 border-b border-slate-700 bg-slate-800">
                        <h2 className="text-lg font-bold text-slate-100">Eventos Registrados</h2>
                    </div>

                    <div className="p-4">
                        <table className="w-full text-left border-collapse">
                            <thead>
                                <tr className="border-b border-slate-700 text-orange-400">
                                    <th className="p-3">Título</th>
                                    <th className="p-3">Categoría</th>
                                    <th className="p-3">Fecha</th>
                                    <th className="p-3">Ubicación</th>
                                    <th className="p-3">Boletos (Disp/Total)</th>
                                    <th className="p-3">Precio</th>
                                    <th className="p-3 text-center">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                {loading ? (
                                    <tr>
                                        <td colSpan={7} className="p-4 text-center">Cargando...</td>
                                    </tr>
                                ) : events.length === 0 ? (
                                    <tr>
                                        <td colSpan={7} className="p-4 text-center">No hay eventos.</td>
                                    </tr>
                                ) : (
                                    events.map((ev) => (
                                        <tr key={ev.id} className="border-b border-slate-700/50 hover:bg-slate-700/30">
                                            <td className="p-3 font-bold text-slate-100">{ev.title}</td>
                                            <td className="p-3">{ev.categoryName}</td>
                                            <td className="p-3">{new Date(ev.date).toLocaleDateString()}</td>
                                            <td className="p-3">{ev.location}</td>
                                            <td className="p-3">
                                                {ev.availableTickets} / {ev.totalTickets}
                                            </td>
                                            <td className="p-3 font-bold text-green-400">${ev.price}</td>
                                            <td className="p-3 text-center space-x-2">
                                                <button className="bg-blue-600 text-white px-2 py-1 rounded text-sm hover:bg-blue-700">Editar</button>
                                                <button onClick={() => handleDelete(ev.id)} className="bg-red-600 text-white px-2 py-1 rounded text-sm hover:bg-red-700">Eliminar</button>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};
