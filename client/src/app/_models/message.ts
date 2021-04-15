export interface Message {
    id: number;
    senderID: number;
    recipientID: number;
    senderUsername: string;
    senderPhotoUrl: string;
    recipientUsername: string;
    recipientPhotoUrl: string;
    content: string;
    dateRead?: Date;
    dateSent: Date;
}