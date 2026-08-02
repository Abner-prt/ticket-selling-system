import { ticketApi } from "../api/ticketApi";
import { EventDto, EventCreateDto, CategoryDto, CategoryCreateDto, PageDto, ResponseDto } from "../../infrastructure/interfaces/DTOs";

// --- Acciones de Eventos ---
export const getEventsAction = async (page: number = 1, searchTerm: string = "") => {
    const { data } = await ticketApi.get<ResponseDto<PageDto<EventDto[]>>>(`/event?page=${page}&searchTerm=${searchTerm}`);
    return data;
}

export const createEventAction = async (event: EventCreateDto) => {
    const { data } = await ticketApi.post<ResponseDto<EventDto>>('/event', event);
    return data;
}

export const deleteEventAction = async (id: number) => {
    const { data } = await ticketApi.delete<ResponseDto<EventDto>>(`/event/${id}`);
    return data;
}

// --- Acciones de Categorías ---
export const getCategoriesAction = async (page: number = 1, searchTerm: string = "") => {
    const { data } = await ticketApi.get<ResponseDto<PageDto<CategoryDto[]>>>(`/category?page=${page}&searchTerm=${searchTerm}`);
    return data;
}

export const createCategoryAction = async (category: CategoryCreateDto) => {
    const { data } = await ticketApi.post<ResponseDto<CategoryDto>>('/category', category);
    return data;
}

export const deleteCategoryAction = async (id: number) => {
    const { data } = await ticketApi.delete<ResponseDto<CategoryDto>>(`/category/${id}`);
    return data;
}
