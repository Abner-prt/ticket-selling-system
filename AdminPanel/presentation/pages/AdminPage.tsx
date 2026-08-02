import { useEffect, useState } from 'react';
import { getEventsAction, deleteEventAction, createEventAction, getCategoriesAction, createCategoryAction } from '../../core/actions/admin.actions';
import { EventDto, CategoryDto } from '../../infrastructure/interfaces/DTOs';
import { Trash2, Plus, Calendar, Tag } from 'lucide-react';

export const AdminPage = () => {
  const [events, setEvents] = useState<EventDto[]>([]);
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [loading, setLoading] = useState(true);

  const [newCatName, setNewCatName] = useState('');
  const [newEvent, setNewEvent] = useState({
    title: '', description: '', date: '', location: '', price: 0, totalTickets: 0, categoryId: 0
  });

  const loadData = async () => {
    setLoading(true);
    try {
      const evRes = await getEventsAction(1, '');
      const catRes = await getCategoriesAction(1, '');
      setEvents(evRes.data.items || []);
      setCategories(catRes.data.items || []);
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const handleCreateCategory = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newCatName) return;
    await createCategoryAction({ name: newCatName });
    setNewCatName('');
    loadData();
  };

  const handleCreateEvent = async (e: React.FormEvent) => {
    e.preventDefault();
    if (newEvent.categoryId === 0 && categories.length > 0) {
      newEvent.categoryId = categories[0].id;
    }
    await createEventAction(newEvent);
    setNewEvent({ title: '', description: '', date: '', location: '', price: 0, totalTickets: 0, categoryId: 0 });
    loadData();
  };

  const handleDeleteEvent = async (id: number) => {
    if (confirm('¿Estás seguro de eliminar este evento?')) {
      await deleteEventAction(id);
      loadData();
    }
  };

  if (loading) return <div className="p-10 text-center text-gray-500">Cargando la información...</div>;

  return (
    <div className="container mx-auto p-6 max-w-6xl">
      <h1 className="text-3xl font-bold mb-8 text-gray-800">Panel de Administración (Eventos y Categorías)</h1>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
        
        {/* Sección de Categorías */}
        <div className="bg-white p-6 rounded-lg shadow border border-gray-200">
          <h2 className="text-xl font-bold mb-4 flex items-center gap-2">
            <Tag className="w-5 h-5" /> Categorías
          </h2>
          
          <form onSubmit={handleCreateCategory} className="mb-6 flex gap-2">
            <input 
              type="text" 
              placeholder="Nueva Categoría" 
              className="border p-2 rounded flex-1"
              value={newCatName}
              onChange={(e) => setNewCatName(e.target.value)}
              required
            />
            <button type="submit" className="bg-blue-600 text-white p-2 rounded hover:bg-blue-700">
              <Plus className="w-5 h-5" />
            </button>
          </form>

          <ul className="space-y-2">
            {categories.map(c => (
              <li key={c.id} className="bg-gray-50 p-3 rounded flex justify-between items-center border">
                <span>{c.name}</span>
                <span className="text-xs text-gray-400">ID: {c.id}</span>
              </li>
            ))}
          </ul>
        </div>

        {/* Sección de Eventos */}
        <div className="md:col-span-2 bg-white p-6 rounded-lg shadow border border-gray-200">
          <h2 className="text-xl font-bold mb-4 flex items-center gap-2">
            <Calendar className="w-5 h-5" /> Gestión de Eventos
          </h2>

          <form onSubmit={handleCreateEvent} className="mb-8 grid grid-cols-2 gap-4 bg-gray-50 p-4 rounded border">
            <input type="text" placeholder="Título" required className="border p-2 rounded" value={newEvent.title} onChange={e => setNewEvent({...newEvent, title: e.target.value})} />
            <input type="date" required className="border p-2 rounded" value={newEvent.date} onChange={e => setNewEvent({...newEvent, date: e.target.value})} />
            <input type="text" placeholder="Ubicación" required className="border p-2 rounded" value={newEvent.location} onChange={e => setNewEvent({...newEvent, location: e.target.value})} />
            <input type="number" placeholder="Precio ($)" required className="border p-2 rounded" value={newEvent.price || ''} onChange={e => setNewEvent({...newEvent, price: Number(e.target.value)})} />
            <input type="number" placeholder="Boletos Totales" required className="border p-2 rounded" value={newEvent.totalTickets || ''} onChange={e => setNewEvent({...newEvent, totalTickets: Number(e.target.value)})} />
            
            <select className="border p-2 rounded" required value={newEvent.categoryId} onChange={e => setNewEvent({...newEvent, categoryId: Number(e.target.value)})}>
              <option value={0} disabled>Selecciona una categoría</option>
              {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>

            <textarea placeholder="Descripción" className="border p-2 rounded col-span-2" value={newEvent.description} onChange={e => setNewEvent({...newEvent, description: e.target.value})} />
            
            <button type="submit" className="col-span-2 bg-green-600 text-white py-2 rounded hover:bg-green-700 font-bold transition-colors">
              Crear Nuevo Evento
            </button>
          </form>

          <div className="space-y-4">
            {events.map(ev => (
              <div key={ev.id} className="border p-4 rounded-lg flex justify-between items-center hover:bg-gray-50 transition-colors">
                <div>
                  <h3 className="font-bold text-lg text-gray-800">{ev.title}</h3>
                  <p className="text-sm text-gray-600">{ev.date.split('T')[0]} - {ev.location}</p>
                  <span className="inline-block bg-blue-100 text-blue-800 text-xs px-2 py-1 rounded mt-1 font-semibold">
                    {ev.categoryName}
                  </span>
                </div>
                <div className="flex items-center gap-4">
                  <div className="text-right">
                    <p className="font-bold text-green-600">${ev.price}</p>
                    <p className="text-xs text-gray-500">{ev.availableTickets}/{ev.totalTickets} tickets</p>
                  </div>
                  <button onClick={() => handleDeleteEvent(ev.id)} className="text-red-500 hover:bg-red-100 p-2 rounded-full transition-colors">
                    <Trash2 className="w-5 h-5" />
                  </button>
                </div>
              </div>
            ))}
            {events.length === 0 && <p className="text-gray-500 text-center italic">Aún no hay eventos registrados.</p>}
          </div>

        </div>
      </div>
    </div>
  );
};
