import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private readonly hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5162/chathub')
      .build();
  }

  // Connect to the SignalR hub
  async connect(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('SignalR connected');
    } catch (error) {
      console.error('SignalR connection error:', error);
    }
  }

  // Start a chat session between two users
  async startChat(userId: string, targetUserId: string): Promise<void> {
    try {
      await this.hubConnection.invoke('StartChat', userId, targetUserId);
    } catch (error) {
      console.error('Error invoking StartChat:', error);
    }
  }

  // Listen for "ChatStarted" events
  addChatStartedListener(callback: (userId: string, targetUserId: string) => void): void {
    this.hubConnection.on('ChatStarted', callback);
  }

  // Register the user with their userId
  async registerUser(userId: string): Promise<void> {
    try {
      await this.hubConnection.invoke('RegisterUser', userId);
    } catch (error) {
      console.error('Error invoking RegisterUser:', error);
    }
  }

  // Send a private message to a specific user
  async sendMessage(receiverId: string, senderId: string, message: string): Promise<void> {
    try {
      await this.hubConnection.invoke('SendPrivateMessage', senderId, receiverId, message);
    } catch (error) {
      console.error('Error invoking SendPrivateMessage:', error);
    }
  }

  // Listen for incoming messages
  addMessageListener(callback: (senderId: string, message: string) => void): void {
    this.hubConnection.on('ReceiveMessage', callback);
  }

  // Listen for user not available responses
  addUserNotAvailableListener(callback: (receiverId: string) => void): void {
    this.hubConnection.on('UserNotAvailable', callback);
  }
}