import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Message } from '../features/chat-2/message';
import { map, Observable, pipe } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private readonly hubConnection: HubConnection;

  constructor(private http: HttpClient) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5162/chathub')
      .build();
  }

  async connect(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('SignalR connected');
    } catch (error) {
      console.error('SignalR connection error:', error);
    }
  }

  async startChat(userId: number, targetUserId: number): Promise<void> {
    try {
      await this.hubConnection.invoke('StartChat', userId, targetUserId);
    } catch (error) {
      console.error('Error invoking StartChat:', error);
    }
  }

  addChatStartedListener(callback: (userId: number, targetUserId: number) => void): void {
    this.hubConnection.on('ChatStarted', callback);
  }

  async registerUser(userId: number): Promise<void> {
    try {
      await this.hubConnection.invoke('RegisterUser', userId);
    } catch (error) {
      console.error('Error invoking RegisterUser:', error);
    }
  }

  async sendMessage(receiverId: number, senderId: number, message: string): Promise<void> {
    try {
      await this.hubConnection.invoke('SendPrivateMessage', senderId, receiverId, message);
    } catch (error) {
      console.error('Error invoking SendPrivateMessage:', error);
    }
  }

  addMessageListener(callback: (senderId: number, message: string) => void): void {
    this.hubConnection.on('ReceiveMessage', callback);
  }

  addUserNotAvailableListener(callback: (receiverId: number) => void): void {
    this.hubConnection.on('UserNotAvailable', callback);
  }

  getMessages(currentUserId: number, targetUserId: number): Observable<Message[]> {
    return this.http.get<any>(`http://localhost:5162/api/chat/get-messages/${currentUserId}/${targetUserId}`).pipe(
      map(response => response.data) 
    );
  }
}