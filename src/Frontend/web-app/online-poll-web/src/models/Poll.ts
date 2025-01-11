export interface Poll {
    id: number;
    title: string;
    description: string;
    createdAt: string;
    startDate: string;
    endDate: string;
    isActive: boolean;
    creatorId: number;
    options: PollOption[];
}

export interface PollOption {
    id: number;
    pollId: number;
    optionText: string;
    voteCount: number;
}

export interface PollResult {
    pollId: number;
    title: string;
    optionResults: PollOptionResult[];
}

export interface PollOptionResult {
    optionId: number;
    optionText: string;
    voteCount: number;
    percentage: number;
}

export interface PaginationResult<T> {
    items: T[];
    totalItems: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
}