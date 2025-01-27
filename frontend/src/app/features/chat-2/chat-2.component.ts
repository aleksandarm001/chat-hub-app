import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { Message } from './message';
import { User } from '../models/user';
import { UsersService } from 'src/app/services/users.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat-2.component.html',
  styleUrls: ['./chat-2.component.css'],
})
export class Chat2Component implements OnInit {
  messages: Message[] = [];
  users: User[] = [];
  activeUserUsername: string | null = null;
  currentUserId = ''; 
  newMessage = '';

  constructor(private chatService: ChatService,
    private userService: UsersService,
    private authService : AuthService) {}

  async ngOnInit() {
    
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
      },
      error: (err) => {
        console.error('Error fetching users:', err);
      },
    });

    this.authService.user$.subscribe({
      next: (user) => {
        this.currentUserId = user.username;
        console.log("current user id: ", this.currentUserId);
      },
      error: (err) => {
        console.error('Error');
      }
    })

    
    await this.chatService.connect();


    await this.chatService.registerUser(this.currentUserId);

    this.chatService.addMessageListener((senderId, message) => {
      const receivedMessage: Message = {
        senderId,
        receiverId: this.currentUserId,
        content: message,
        timestamp: new Date(),
      };
      this.messages.push(receivedMessage);
    });

    this.chatService.addUserNotAvailableListener((receiverId) => {
      alert(`User with ID ${receiverId} is not available.`);
    });

    this.chatService.addChatStartedListener((userId, targetUserId) => {
      if (userId === this.currentUserId) {
        console.log(`Chat started with ${targetUserId}`);
      }
    });
  }

  onUserClick(targetUserId: string): void {
    this.activeUserUsername = targetUserId;
    this.chatService.startChat(this.currentUserId, targetUserId);
    this.loadMessages(targetUserId);
  }

  setActiveUser(userId: string) {
    this.activeUserUsername = userId;
    this.loadMessages(userId);
  }

  loadMessages(userId: string) {
    this.messages = []; 
  }

  async sendMessage() {
    if (this.activeUserUsername && this.newMessage.trim()) {
      const message: Message = {
        senderId: this.currentUserId,
        receiverId: this.activeUserUsername,
        content: this.newMessage,
        timestamp: new Date(),
      };

      await this.chatService.sendMessage(
        this.activeUserUsername,
        this.currentUserId,
        this.newMessage
      );

      this.messages.push(message);
      this.newMessage = '';
    }
  }
}