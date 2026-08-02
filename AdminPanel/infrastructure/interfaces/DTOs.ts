export interface ResponseDto<T> {
    message: string;
    status: boolean;
    data: T;
}

export interface PageDto<T> {
    currentPage: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
    pageSize: number;
    totalItems: number;
    totalPages: number;
    items: T;
}

export interface EventDto {
    id: number;
    title: string;
    description: string;
    date: string;
    location: string;
    price: number;
    totalTickets: number;
    availableTickets: number;
    categoryId: number;
    categoryName: string;
}

export interface EventCreateDto {
    title: string;
    description: string;
    date: string;
    location: string;
    price: number;
    totalTickets: number;
    categoryId: number;
}

export interface CategoryDto {
    id: number;
    name: string;
}

export interface CategoryCreateDto {
    name: string;
}
